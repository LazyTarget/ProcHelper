using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl.API.Data;
using FullCtrl.Base;
using Lux.Extensions;

namespace FullCtrl.API
{
    public class FullCtrlServer : IDisposable
    {
        private readonly IPluginStore _pluginStore;
        private readonly IDictionary<string, IPlugin> _plugins;
        private bool _started;
        private bool _disposed;

        public FullCtrlServer()
        {
            _plugins = new Dictionary<string, IPlugin>();
            _pluginStore = new PluginLoader();
        }


        private void LoadPlugins()
        {
            lock (_plugins)
            {
                _plugins.Clear();
                var plugs = _pluginStore.GetPlugins().WaitForResult();
                if (plugs != null)
                {
                    foreach (var plugin in plugs)
                    {
                        _plugins[plugin.Name] = plugin;
                    }
                }
            }
        }


        public void Start()
        {
            if (_started)
                return;
            _started = true;
            LoadPlugins();
        }

        public void Stop()
        {
            _started = false;
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
