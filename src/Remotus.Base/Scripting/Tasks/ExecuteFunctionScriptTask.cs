using System;
using System.Linq;
using System.Threading.Tasks;

namespace Remotus.Base.Scripting
{
    public class ExecuteFunctionScriptTask : ScriptTaskBase
    {
        public override string Name => "ExecuteFunction";

        public string PluginID { get; set; }

        public string FunctionID { get; set; }

        public IFunctionArguments Arguments { get; set; }


        public override async Task<IResponseBase> Execute(IExecutionContext context)
        {
            var plugins = await context.ClientInfo.GetPlugins();
            var plugin = plugins?.FirstOrDefault(x => string.Equals(x.ID, PluginID, StringComparison.OrdinalIgnoreCase));
            var functionPlugin = plugin as IFunctionPlugin;
            var functionDescriptor = functionPlugin?.GetFunctions()?.FirstOrDefault(x => string.Equals(x.ID, FunctionID, StringComparison.OrdinalIgnoreCase));
            if (functionDescriptor != null)
            {
                try
                {
                    using (var function = functionDescriptor?.Instantiate())
                    {
                        var result = await function.Execute(context, Arguments);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    var result = DefaultResponseBase.CreateError(DefaultError.FromException(ex));
                    return result;
                }
            }
            return null;
        }
    }
}