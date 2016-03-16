using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using FullCtrl.Base;
using FullCtrl.Plugins.Sound;
using Lux.IO;
using Remotus.Plugins.Process;

namespace FullCtrl.API.Data
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
            plugins.Add(new ProcessManagerPlugin());
            return plugins;
        }
    }
}
