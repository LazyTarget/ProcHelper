using System.Threading.Tasks;

namespace Remotus.API
{
    public abstract class HubAction
    {
        public abstract Task Invoke(object[] arguments);
    }
}