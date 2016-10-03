using System;
using System.Linq;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public class InvokeFunctionHubAction : HubAction
    {
        public string PluginID { get; set; }
        public string FunctionID { get; set; }

        public override Task Invoke(IExecutionContext context, object[] arguments)
        {
            var args = arguments.FirstOrDefault() as IFunctionArguments;
            var funcID = FunctionID ?? args?.Descriptor?.ID;
            var task = context?.Remotus?.ExecuteLocalFunction(PluginID, funcID, args);
            return task;
        }
    }
}