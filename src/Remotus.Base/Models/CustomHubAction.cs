using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public class CustomHubAction : HubAction
    {
        public Func<IExecutionContext, object[], Task> Action { get; set; }
        
        public override Task Invoke(IExecutionContext context, object[] arguments)
        {
            var task = Action?.Invoke(context, arguments);
            return task;
        }
    }
}