using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.API.Data
{
    public class StockPluginLoader : IPluginStore
    {
        private readonly IList<LoadedPlugin> _plugins = new List<LoadedPlugin>();
        private bool _loaded;

        public StockPluginLoader()
        {

        }

        private async Task LoadPlugins()
        {
            if (_loaded)
                return;
            lock (_plugins)
            {
                if (_loaded)
                    return;

                _plugins.Clear();
                _plugins.Add(LoadPlugin(new Plugins.Sound.SoundPlugin()));
                _plugins.Add(LoadPlugin(new Plugins.Process.ProcessPlugin()));
                _plugins.Add(LoadPlugin(new Plugins.Services.ServicesPlugin()));
                _plugins.Add(LoadPlugin(new Plugins.Spotify.SpotifyPlugin()));
                _plugins.Add(LoadPlugin(new Plugins.Scripting.ScriptingPlugin()));
                //_plugins.Add(LoadPlugin(new Plugins.Input.MouseInputPlugin()));
                //_plugins.Add(LoadPlugin(new Plugins.Input.KeyboardInputPlugin()));

                _loaded = true;
            }
        }

        private LoadedPlugin LoadPlugin(IPlugin plugin)
        {
            var p = new LoadedPlugin
            {
                Instance = plugin,
            };
            return p;
        }
        
        public async Task<IEnumerable<LoadedPlugin>> GetPlugins()
        {
            await LoadPlugins();
            return _plugins.AsEnumerable();
        }
    }
}
