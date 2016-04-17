using System.Collections.Generic;
using System.Linq;
using Remotus.Base;

namespace Remotus.Plugins.Scripting
{
    public class ScriptingPlugin : IFunctionPlugin
    {
        //private IScriptDataStore _dataStore = new MemoryScriptDataStore();
        private IScriptDataStore _dataStore = new FileSystemScriptDataStore();

        public string ID        => "A1521C38-D4BA-4512-8A41-FF6FCF99F937";
        public string Name      => "Scripting";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            var scripts = _dataStore.GetScripts();
            var descriptors = scripts?.Select(ScriptFunctionDescriptor.FromScript);
            return descriptors;
        }
    }
}
