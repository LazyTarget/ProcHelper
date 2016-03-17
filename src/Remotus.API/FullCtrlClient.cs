using System;
using System.Collections.Generic;
using System.Linq;
using FullCtrl.API.Config;
using FullCtrl.API.Data;
using FullCtrl.API.Models;
using FullCtrl.Base;
using Lux.Extensions;
using Microsoft.Owin.Hosting;

namespace FullCtrl.API
{
    public class FullCtrlClient : IDisposable
    {
        private readonly IPluginStore _pluginStore;
        private readonly IDictionary<string, IPlugin> _plugins;
        private bool _started;
        private bool _disposed;
        private IClientInfo _clientInfo;
        private IDisposable _server;

        public FullCtrlClient()
        {
            _plugins = new Dictionary<string, IPlugin>();
            _pluginStore = new PluginLoader();
        }

        public ClientConfig Config { get; private set; }

        public IClientInfo ClientInfo { get { return _clientInfo; } }


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
            return _plugins.Values;
        }


        protected virtual StartOptions GetStartOptions(ClientConfig config)
        {
            var options = new StartOptions();
            options.Urls.Add("http://+:9000");
            options.Urls.Add("http://localhost:9000");
            return options;
        }

        protected virtual IDisposable StartAsSelftHost(StartOptions options)
        {
            var res = WebApp.Start<Startup>(options);
            return res;
        }


        public virtual void Start(ClientConfig config)
        {
            if (_started)
                return;
            _started = true;


            // Info
            _clientInfo = new ClientInfo
            {
                ApiAddress = Config?.ClientApiAddress,
                ClientID = Guid.NewGuid().ToString(),
                ApiVersion = 1,
                ServerInfo = new ServerInfo
                {
                    ApiAddress = Config?.ServerApiAddress,
                    ApiVersion = 1,
                },
            };


            // Start WebAPI
            Console.WriteLine("Server starting");

            var options = GetStartOptions(config);
            _server = StartAsSelftHost(options);

            Console.WriteLine("Server started");


            // Plugins
            LoadPlugins();

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
        }


        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            try
            {
                Stop();

                _plugins.Clear();

                _server?.Dispose();
                _server = null;
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}