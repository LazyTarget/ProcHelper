using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Remotus.Base.Net;

namespace Remotus.API.Net.Hubs
{
    public class CustomHub : HubBase
    {
        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;

        

        public void InvokeCustom(ExternalHubMessage message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message?.HubName))
                    throw new ArgumentNullException(nameof(message.HubName));

                var customHubIdentifier = $"CustomHub_{message.HubName}";
                var group = Clients.Group(customHubIdentifier);
                var gp = (IClientProxy) group;

                var args = message?.Args?.Length > 0
                    ? new object[] { message.Args.ToArray() }
                    : new object[0];
                var timeout = TimeSpan.FromSeconds(15);
                var task = gp.Invoke(message.Method, args);
                task.Wait(timeout);
            }
            catch (Exception ex)
            {
                
            }
        }



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
