using System.Threading.Tasks;

namespace Remotus.Base
{
    public abstract class HubAction
    {
        public abstract Task Invoke(IExecutionContext context, object[] arguments);
    }
}