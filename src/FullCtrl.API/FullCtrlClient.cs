using System;
using System.Collections.Generic;
using System.Linq;
using FullCtrl.API.Config;
using FullCtrl.API.Data;
using FullCtrl.API.Models;
using FullCtrl.Base;
using Lux.Config.Xml;
using Lux.Extensions;

namespace FullCtrl.API
{
    public class FullCtrlClient : IDisposable
    {
        private readonly IPluginStore _pluginStore;
        private readonly IDictionary<string, IPlugin> _plugins;
        private bool _started;
        private bool _disposed;
        private IClientInfo _clientInfo;

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

        public void Start()
        {
            if (_started)
                return;
            _started = true;

            // Config
            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<ClientConfig>();
            Config = Lux.Framework.ConfigManager.Load<ClientConfig>(descriptor);
            
            _clientInfo = new ClientInfo
            {
                ApiAddress = Config.ClientApiAddress,
                ClientID = Guid.NewGuid().ToString(),
                ApiVersion = 1,
                ServerInfo = new ServerInfo
                {
                    ApiAddress = Config.ServerApiAddress,
                    ApiVersion = 1,
                },
            };

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
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            Stop();
            _plugins.Clear();
            _disposed = true;
        }
    }
}