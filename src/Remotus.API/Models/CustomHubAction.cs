using System;
using System.Threading.Tasks;

namespace Remotus.API
{
    public class CustomHubAction : HubAction
    {
        public Func<object[], Task> Action { get; set; }

        public override Task Invoke(object[] arguments)
        {
            var task = Action?.Invoke(arguments);
            return task;
        }
    }
}