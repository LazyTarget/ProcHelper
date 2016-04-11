using System.Collections.Generic;
using System.Linq;
using Remotus.Base;
using Remotus.Base.Scripting;

namespace Remotus.Plugins.Scripting
{
    public class MemoryScriptDataStore : IScriptDataStore
    {
        public MemoryScriptDataStore()
        {
            Scripts = new List<Script>();

            var testScript = new Script
            {
                ID = "3FE0E633-771F-4B75-BFB7-BE520EC11FF1",
                Name = "Test script",
                Version = "1.0.0.0",
                Tasks = new ScriptTaskBase[]
                {
                    new ExecuteFunctionScriptTask
                    {
                        PluginID = "ABA6417A-65A2-4761-9B01-AA9DFFC074C0",
                        FunctionID = "78324B37-0F93-40C2-AC56-5B1D714CFC41",
                        Arguments = null,
                    },
                },
            };
            Scripts.Add(testScript);
        }

        public List<Script> Scripts { get; set; }

        public IEnumerable<Script> GetScripts()
        {
            return Scripts?.AsEnumerable() ?? new Script[0];
        }
    }
}