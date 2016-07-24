using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.API.Hubs
{
    public class AgentHub : Hub
    {
        public override Task OnConnected()
        {
            HubServer.Instance.ConnectionManager.OnConnected(this);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            HubServer.Instance.ConnectionManager.OnReconnected(this);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            HubServer.Instance.ConnectionManager.OnDisconnected(this, stopCalled);
            return base.OnDisconnected(stopCalled);
        }
    }
}
