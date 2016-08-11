using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lux.IO;
using Remotus.Base;

namespace Remotus.API.Data
{
    public class PluginLauncher : IPluginStore
    {
        private readonly IList<AgentPlugin> _plugins = new List<AgentPlugin>();
        private bool _loaded;

        public IFileSystem FileSystem { get; set; }

        public string Directory { get; set; }


        public PluginLauncher()
        {
            FileSystem = new FileSystem();
            Directory = AppDomain.CurrentDomain.BaseDirectory;
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

                var instantiator = new Lux.TypeInstantiator();
                var helper = new FileSystemHelper(FileSystem);
                var filePatterns = new string[]
                {
                    "*Plugin*.dll",
                    "*Plugins*.dll",
                    "*plugin*.dll",
                    "*.Plugin",
                    "*.plugin"
                };
                var pattern = Directory.Trim().Trim('/', '\\') + string.Join("|", filePatterns);
                var paths = helper.FindFilesWildcard(pattern)?.ToList();
                if (paths != null && paths.Any())
                {
                    foreach (var filePath in paths)
                    {
                        var plugin = new LoadedPlugin
                        {
                            //ID = instance.ID,
                            Name = filePath,
                            //Version = instance.Version,
                            Loaded = false,
                            //Instance = instance,
                            PluginFile = filePath,
                            //PluginInstanceType = pluginType,
                        };
                        _plugins.Add(plugin);
                    }
                }
                else
                {

                }
                _loaded = true;
            }
        }

        public async Task<IEnumerable<AgentPlugin>> GetPlugins()
        {
            await LoadPlugins();
            return _plugins.AsEnumerable();
        }
    }
}
