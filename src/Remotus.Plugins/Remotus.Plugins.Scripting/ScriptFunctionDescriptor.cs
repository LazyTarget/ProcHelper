using System.Linq;
using Remotus.Base;
using Remotus.Base.Scripting;

namespace Remotus.Plugins.Scripting
{
    public class ScriptFunctionDescriptor : IFunctionDescriptor
    {
        private ScriptFunctionDescriptor(Script script)
        {
            Script = script;
        }

        public Script Script { get; private set; }

        public string ID => Script?.ID;
        public string Name => Script?.Name;
        public string Version => Script?.Version;

        public IFunction Instantiate()
        {
            var function = new ScriptFunction(this);
            return function;
        }

        public IParameterCollection GetParameters()
        {
            IParameterCollection res = null;
            var executableScriptTask = Script?.Tasks?.OfType<ExecuteFunctionScriptTask>().ToArray();
            if (executableScriptTask != null)
            {
                foreach (var executeFunctionScriptTask in executableScriptTask)
                {
                    var parameters = executeFunctionScriptTask?.Arguments?.Parameters;
                    if (parameters != null && parameters.Any())
                    {
                        if (res == null)
                        {
                            res = new ParameterCollection();
                        }

                        foreach (var parameter in parameters)
                        {
                            if (string.IsNullOrWhiteSpace(parameter.Key))
                                continue;
                            res[parameter.Key] = parameter.Value;
                        }
                    }
                }
            }
            return res;
        }


        public static ScriptFunctionDescriptor FromScript(Script script)
        {
            var descriptor = new ScriptFunctionDescriptor(script);
            return descriptor;
        }
    }
}