using System;
using System.Threading.Tasks;

namespace Remotus.Base.Scripting
{
    public class ExecuteFunctionScriptTask : ScriptTaskBase
    {
        public override string Name => "ExecuteFunction";

        public string ClientID { get; set; }

        public string PluginID { get; set; }

        public string FunctionID { get; set; }

        public IFunctionArguments Arguments { get; set; }


        public override async Task<IResponseBase> Execute(IExecutionContext context)
        {
            try
            {
                var response = ClientID != null
                    ? await context.Remotus.ExecuteRemoteFunction(ClientID, PluginID, FunctionID, Arguments)
                    : await context.Remotus.ExecuteLocalFunction(PluginID, FunctionID, Arguments);
                return response;
            }
            catch (Exception ex)
            {
                var result = DefaultResponseBase.CreateError(DefaultError.FromException(ex));
                return result;
            }
        }
    }
}