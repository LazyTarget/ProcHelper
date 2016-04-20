using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.API.Data
{
    public class StockPluginLoader : IPluginStore
    {
        public StockPluginLoader()
        {

        }
        
        public async Task<IEnumerable<IPlugin>> GetPlugins()
        {
            var plugins = new List<IPlugin>();
            plugins.Add(new Plugins.Sound.SoundPlugin());
            plugins.Add(new Plugins.Process.ProcessPlugin());
            plugins.Add(new Plugins.Services.ServicesPlugin());
            plugins.Add(new Plugins.Spotify.SpotifyPlugin());
            plugins.Add(new Plugins.Scripting.ScriptingPlugin());
            plugins.Add(new Plugins.Input.MouseInputPlugin());
            plugins.Add(new Plugins.Input.KeyboardInputPlugin());
            return plugins;
        }
    }
}
