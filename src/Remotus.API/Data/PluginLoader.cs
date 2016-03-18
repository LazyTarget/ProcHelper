using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lux.IO;
using Remotus.Base;
using Remotus.Plugins.Process;
using Remotus.Plugins.Sound;

namespace Remotus.API.Data
{
    public class PluginLoader : IPluginStore
    {
        public IFileSystem FileSystem { get; set; }

        public string Directory { get; set; }


        public PluginLoader()
        {
            FileSystem = new FileSystem();
            Directory = AppDomain.CurrentDomain.BaseDirectory;
        }


        public async Task<IEnumerable<IPlugin>> GetPlugins()
        {
            var plugins = new List<IPlugin>();

            // todo: populate via assembly load

            plugins.Add(new SoundFunctionPlugin());
            plugins.Add(new ProcessFunctionPlugin());
            return plugins;
        }
    }
}
