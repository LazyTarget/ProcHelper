using System;
using System.Collections.Generic;
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

        

        public void InvokeCustom(CustomHubMessage message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message?.HubName))
                    throw new ArgumentNullException(nameof(message.HubName));

                var args = message?.Args?.Length > 0
                    ? new object[] { message.Args.ToArray() }
                    : new object[0];

                Task task;
                var timeout = TimeSpan.FromSeconds(15);
                var groupNames = message?.Groups?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList() ?? new List<string>();
                if (groupNames.Any())
                {
                    var group = Clients.Groups(groupNames);
                    var proxy = (IClientProxy) group;
                    task = proxy.Invoke(message.Method, args);
                }
                else
                {
                    var all = Clients.All;
                    var proxy = (IClientProxy)all;
                    task = proxy.Invoke(message.Method, args);
                }
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
