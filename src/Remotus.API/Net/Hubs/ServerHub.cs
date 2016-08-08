using System.Reflection;
using System.Threading.Tasks;

namespace Remotus.API.Net.Hubs
{
    public class ServerHub : HubBase
    {
        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;


        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}
