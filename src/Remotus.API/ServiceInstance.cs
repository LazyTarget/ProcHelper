using System;
using System.Collections.Generic;
using System.Linq;
using Lux.Config.Xml;
using Lux.Extensions;
using Microsoft.Owin.Hosting;
using Remotus.API.Data;
using Remotus.Base;

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
        private readonly IDictionary<string, IPlugin> _plugins;

        public ServiceInstance()
        {
            _clients = new Dictionary<string, IClientInfo>();
            _plugins = new Dictionary<string, IPlugin>();
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
                            _plugins[plugin.Name] = plugin;
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
                result = _plugins.Values.ToList();
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
                var servicePlugins = _plugins.Values.OfType<IServicePlugin>().ToList();
                foreach (var servicePlugin in servicePlugins)
                {
                    try
                    {
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
