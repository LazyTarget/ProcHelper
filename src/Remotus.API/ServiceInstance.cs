﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lux.Config.Xml;
using Lux.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Remotus.API.Data;
using Remotus.API.Hubs;
using Remotus.API.Hubs.Client;
using Remotus.API.v1;
using Remotus.Base;
using Remotus.Base.Models.Payloads;

namespace Remotus.API
{
    internal class ServiceInstance : IDisposable
    {
        private bool _started;
        private bool _disposed;
        private IDisposable _server;
        private ApiConfig _apiConfig;
        private StartupConfig _startupConfig;

        // Server
        private readonly IDictionary<string, IClientInfo> _clients;
        private IServerInfo _serverInfo;

        // Client
        private IClientInfo _clientInfo;
        private readonly IPluginStore _pluginStore;
        private readonly IDictionary<string, LoadedPlugin> _plugins;

        public ServiceInstance()
        {
            _clients = new Dictionary<string, IClientInfo>();
            _plugins = new Dictionary<string, LoadedPlugin>();
            _pluginStore = new PluginLoader();
        }

        public IClientInfo ClientInfo { get { return _clientInfo; } }


        internal static ApiConfig LoadApiConfig()
        {
            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<ApiConfig>();
            var config = Lux.Framework.ConfigManager.Load<ApiConfig>(descriptor);
            return config;
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
                            if (plugin == null || plugin.Instance == null)
                                continue;
                            if (string.IsNullOrWhiteSpace(plugin.Instance.Name))
                                continue;

                            var servicePlugin = plugin.Instance as IServicePlugin;
                            if (servicePlugin != null)
                            {
                                servicePlugin.OnStatusChanged -= ServicePlugin_OnStatusChanged;
                                servicePlugin.OnStatusChanged += ServicePlugin_OnStatusChanged;
                            }
                            _plugins[plugin.Instance.Name] = plugin;
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
                result = _plugins.Values.Select(x => x.Instance).ToList();
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

            Console.WriteLine("Server started");

            
            // Plugins
            LoadPlugins();

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

                var servicePlugins = _plugins.Values.OfType<IServicePlugin>().ToList();
                foreach (var servicePlugin in servicePlugins)
                {
                    // todo: Make plugin initialization async (load multiple plugins at the same time)

                    try
                    {
                        if (servicePlugin.Status != ServiceStatus.Initializing)
                            servicePlugin.Init(context);

                        if (servicePlugin.Status != ServiceStatus.Running)
                            servicePlugin.Start();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

                
                if (_startupConfig?._Configuration?.Routes != null)
                {
                    var webPlugins = _plugins.Values.OfType<IWebPlugin>().ToList();
                    foreach (var webPlugin in webPlugins)
                    {
                        try
                        {
                            webPlugin.RegisterRoutes(_startupConfig._Configuration.Routes);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }


        public void Stop()
        {
            // Stop plugins
            lock (_plugins)
            {
                _started = false;

                var servicePlugins = _plugins.Values.OfType<IServicePlugin>().ToList();
                foreach (var servicePlugin in servicePlugins)
                {
                    try
                    {
                        if (servicePlugin.Status != ServiceStatus.Stopped)
                            servicePlugin.Stop();
                    }
                    catch (Exception ex)
                    {
                        
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
            var componentDesc = new ComponentDescriptor(plugin);
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
