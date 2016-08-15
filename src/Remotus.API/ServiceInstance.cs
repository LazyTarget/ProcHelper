using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lux.Config.Xml;
using Lux.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Remotus.API.Data;
using Remotus.API.Net.Client;
using Remotus.API.Net.Hubs;
using Remotus.API.v1;
using Remotus.Base;
using Remotus.Base.Payloads;

namespace Remotus.API
{
    internal class ServiceInstance : IDisposable
    {
        private bool _started;
        private bool _disposed;
        private IDisposable _server;
        private ApiConfig _apiConfig;
        private StartupConfig _startupConfig;
        private bool _launchPlugins = true;

        // Server
        private readonly IDictionary<string, IClientInfo> _clients;
        private IServerInfo _serverInfo;

        // Client
        private IClientInfo _clientInfo;
        private readonly IPluginStore _pluginStore;
        internal readonly IDictionary<string, AgentPlugin> _plugins;

        public ServiceInstance()
        {
            _clients = new Dictionary<string, IClientInfo>();
            _plugins = new Dictionary<string, AgentPlugin>();
            //_pluginStore = new PluginLoader();
            _pluginStore = new PluginLauncher();
        }

        public IClientInfo ClientInfo { get { return _clientInfo; } }


        internal static ApiConfig LoadApiConfig()
        {
            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<ApiConfig>();
            var config = Lux.Framework.ConfigManager.Load<ApiConfig>(descriptor);
            return config;
        }


        internal void StartPlugin(AgentPlugin plugin)
        {
            try
            {
                // Run Launcher, to Init/Start plugin
                var path = plugin.PluginFile;
                var args = $"plugin {path}";
                var exe = @"E:\Programming\Repos\GitHub\LazyTarget\ProcHelper\src\Remotus.Launcher\bin\Debug\Remotus.Launcher.exe";
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = new ProcessStartInfo(exe, args);
                proc.Exited += delegate (object sender, EventArgs eventArgs)
                {
                    Trace.WriteLine("Plugin launcher exited. Plugin file: " + path);
                    var p = Program.Service._plugins.ContainsKey(plugin.Name)
                        ? Program.Service._plugins[plugin.Name]
                        : null;
                    if (p != null)
                    {
                        p.Status = ServiceStatus.Stopped;
                    }
                };
                proc.EnableRaisingEvents = true;
                Console.WriteLine("Starting plugin launcher. Plugin file: " + path);
                var b = proc.Start();
            }
            catch (Exception ex)
            {

            }
        }


        internal void StopPlugin()
        {

        }


        private void LoadPlugins()
        {
            lock (_plugins)
            {
                _plugins.Clear();
                try
                {
                    var plugs = _pluginStore.GetPlugins().WaitForResult();
                    if (plugs != null)
                    {
                        foreach (var plugin in plugs)
                        {
                            var loadedPlugin = plugin as LoadedPlugin;
                            if (loadedPlugin == null)
                                continue;
                            //if (string.IsNullOrWhiteSpace(loadedPlugin.Name))
                            //    continue;

                            _plugins[plugin.Name] = plugin;
                            if (loadedPlugin.Loaded)
                            {

                            }
                            else
                            {
                                if (loadedPlugin.Status == ServiceStatus.None ||
                                    loadedPlugin.Status == ServiceStatus.Stopped)
                                {
                                    if (_launchPlugins)
                                        StartPlugin(loadedPlugin);
                                }
                                else
                                {
                                    
                                }
                            }

                            //var servicePlugin = loadedPlugin.Instance as IServicePlugin;
                            //if (servicePlugin != null)
                            //{
                            //    servicePlugin.OnStatusChanged -= ServicePlugin_OnStatusChanged;
                            //    servicePlugin.OnStatusChanged += ServicePlugin_OnStatusChanged;
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }


        public IEnumerable<IPlugin> GetPlugins()
        {
            IList<IPlugin> result;
            lock (_plugins)
            {
                //result = _plugins.Values.ToList();
                result = _plugins.Values.Where(x => x.Loaded).Select(x => x.Instance).ToList();
            }
            return result;
        }


        public IEnumerable<IClientInfo> GetClients()
        {
            return _clients.Values;
        }

        public int RegisterClient(IClientInfo clientInfo)
        {
            if (clientInfo == null)
                throw new ArgumentNullException(nameof(clientInfo));
            if (string.IsNullOrWhiteSpace(clientInfo?.ClientID))
                throw new ArgumentException(nameof(clientInfo));

            _clients[clientInfo.ClientID] = clientInfo;
            return 1;
        }
        

        protected virtual StartOptions GetStartOptions(ApiConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var options = new StartOptions();
            //options.Urls.Add("http://+:9001");
            //options.Urls.Add("http://localhost:9001");
            
            foreach (var url in config.Urls)
            {
                options.Urls.Add(url);
            }
            return options;
        }


        protected virtual IDisposable StartAsSelftHost(StartOptions options)
        {
            //var res = WebApp.Start<StartupConfig>(options);
            var res = WebApp.Start(options, (app) =>
            {
                var startup = new StartupConfig();
                startup.Configuration(app);
                _startupConfig = startup;
            });
            return res;
        }


        public virtual void Start(string[] args)
        {
            if (_started)
                return;
            _started = true;
            Console.WriteLine("Starting service instance. Args: " + string.Join(" ", args));

            // Config
            _apiConfig = LoadApiConfig();


            // Info
            var instanceID = Guid.NewGuid().ToString();

            _clientInfo = new ClientInfo
            {
                ApiAddress = _apiConfig?.ClientApiAddress,
                ClientID = instanceID,
                ApiVersion = 1,
                ServerInfo = new ServerInfo
                {
                    ApiAddress = _apiConfig?.ServerApiAddress,
                    ApiVersion = 1,
                },
            };
            _serverInfo = new ServerInfo
            {
                ApiAddress = _apiConfig?.ServerApiAddress,
                ApiVersion = 1,
                InstanceID = instanceID,
            };



            // Start WebAPI
            Console.WriteLine("Server starting");

            var options = GetStartOptions(_apiConfig);
            _server = StartAsSelftHost(options);
            _startupConfig._Configuration.EnsureInitialized();

            Console.WriteLine("Server started");

            
            // Plugins
            LoadPlugins();
            Console.WriteLine("Plugins loaded: " + _plugins.Count);


            //_startupClient?._Configuration?.MapHttpAttributeRoutes();

            lock (_plugins)
            {
                IExecutionContext context = new ExecutionContext
                {
                    ClientInfo = _clientInfo,
                    Logger = new TraceLogger(),
                    Remotus = new Remotus.API.v1.FullCtrlAPI(),
                    //SignalR =  // todo: !
                    HubAgentFactory = new HubAgentFactory()
                };

                var servicePlugins = GetPlugins().OfType<IServicePlugin>().ToList();
                if (servicePlugins.Count > 0)
                {
                    Console.WriteLine("Starting service plugins: " + servicePlugins.Count);
                    foreach (var servicePlugin in servicePlugins)
                    {
                        // todo: Make plugin initialization async (load multiple plugins at the same time)

                        try
                        {
                            if (servicePlugin.Status != ServiceStatus.Initializing)
                            {
                                var task = servicePlugin.Init(context);
                                task.Wait();
                            }

                            if (servicePlugin.Status != ServiceStatus.Running)
                            {
                                var task = servicePlugin.Start();
                                task.Wait();
                            }
                        }
                        catch (Exception ex)
                        {
                        
                        }
                    }
                }

                
                //if (_startupConfig?._Configuration?.Routes != null)
                //{
                //    var webPlugins = _plugins.Values.OfType<IWebPlugin>().ToList();
                //    foreach (var webPlugin in webPlugins)
                //    {
                //        try
                //        {
                //            webPlugin.RegisterRoutes(_startupConfig._Configuration.Routes);
                //        }
                //        catch (Exception ex)
                //        {

                //        }
                //    }
                //}
            }
        }


        public void Stop()
        {
            // Stop plugins
            lock (_plugins)
            {
                _started = false;

                var servicePlugins = GetPlugins().OfType<IServicePlugin>().ToList();
                if (servicePlugins.Count > 0)
                {
                    Console.WriteLine("Stopping service plugins: " + servicePlugins.Count);
                    foreach (var servicePlugin in servicePlugins)
                    {
                        try
                        {
                            if (servicePlugin.Status != ServiceStatus.Stopped)
                            {
                                var task = servicePlugin.Stop();
                                task.Wait();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }


            // Stop WebAPI
            if (_server != null)
            {
                Console.WriteLine("Server stopping");
                _server.Dispose();
                _server = null;
                Console.WriteLine("Server stopped");
            }


            _clients.Clear();
        }


        private void ServicePlugin_OnStatusChanged(object sender, ServiceStatusChangedEventArgs args)
        {
            var agentHub = GlobalHost.ConnectionManager.GetHubContext<AgentHub>();

            var plugin = sender as IServicePlugin;
            var agentId = Program.Service?.ClientInfo?.ClientID;
            var componentDesc = ComponentDescriptor.Create(plugin);
            var model = new PluginStatusChanged(agentId, componentDesc, args.OldStatus, args.NewStatus);


            var msg = string.Format("Plugin '{0}' status changed: {1} => {2}", model.Plugin?.Name, model.OldStatus, model.NewStatus);
            System.Console.WriteLine(msg);
            agentHub.Clients.All.OnPluginStatusChanged(model);


            var msgModel = new DebugMessage(agentId, null, msg, 2);
            var diagHub = GlobalHost.ConnectionManager.GetHubContext<DiagnosticsHub>();
            diagHub.Clients.Group("ListenTo_" + agentId).OnDebugMessage(msgModel);
        }


        public virtual void Dispose()
        {
            if (_disposed)
                return;
            Console.WriteLine("Disposing server");
            _disposed = true;
            try
            {
                Stop();

                _plugins?.Clear();
                _clients?.Clear();

                _server?.Dispose();
                _server = null;
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
