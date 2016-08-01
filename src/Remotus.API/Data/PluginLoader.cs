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
    public class PluginLoader : IPluginStore
    {
        private readonly IList<LoadedPlugin> _plugins = new List<LoadedPlugin>();
        private bool _loaded;

        public IFileSystem FileSystem { get; set; }

        public string Directory { get; set; }


        public PluginLoader()
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
                        // todo: load via web.config?

                        try
                        {
                            var assembly = Assembly.LoadFile(filePath);
                            var pluginTypes =
                                assembly.ExportedTypes.Where(x => typeof(IPlugin).IsAssignableFrom(x)).ToList();

                            foreach (var pluginType in pluginTypes)
                            {
                                var obj = instantiator.Instantiate(pluginType);
                                var instance = (IPlugin)obj;
                                var plugin = new LoadedPlugin
                                {
                                    Instance = instance,
                                };
                                _plugins.Add(plugin);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                else
                {

                }
                _loaded = true;
            }
        }

        public async Task<IEnumerable<LoadedPlugin>> GetPlugins()
        {
            await LoadPlugins();
            return _plugins.AsEnumerable();
        }
    }
}
