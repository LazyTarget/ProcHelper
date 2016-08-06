using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Remotus.Base.Models.Payloads;

namespace Remotus.API.Hubs
{
    public abstract class HubBase : Hub
    {
        public abstract string HubName { get; }


        public async Task AddToGroup(string group)
        {
            if (group == "Events")
            {
                await AddToGroup("Events-Group");
                await AddToGroup("Events-Connections");
                return;
            }

            await Groups.Add(Context.ConnectionId, group);


            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            var model = new HubEvent
            {
                HubName = HubName,
                AgentId = client.Handshake.AgentId,
                ConnectionId = Context.ConnectionId,
                Connected = client.Connected,
                EventName = "AddedToGroup",
                Message = $"ConnectionId '{Context.ConnectionId}' added to group {group} ({HubName})",
            };
            Clients.Group("Events-Group").OnEvent(model);
        }

        public async Task RemoveFromFroup(string group)
        {
            if (group == "Events")
            {
                await RemoveFromFroup("Events-Connections");
                await RemoveFromFroup("Events-Group");
                return;
            }

            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            var model = new HubEvent
            {
                HubName = HubName,
                AgentId = client.Handshake.AgentId,
                ConnectionId = Context.ConnectionId,
                Connected = client.Connected,
                EventName = "RemovedFromGroup",
                Message = $"ConnectionId '{Context.ConnectionId}' removed from group {group} ({HubName})",
            };
            Clients.Group("Events-Group").OnEvent(model);



            await Groups.Remove(Context.ConnectionId, group);
        }




        public override Task OnConnected()
        {
            HubServer.Instance.ConnectionManager.OnConnected(this);



            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            if (client != null)
            {
                var model = new HubEvent
                {
                    HubName = HubName,
                    AgentId = client.Handshake.AgentId,
                    ConnectionId = Context.ConnectionId,
                    Connected = client.Connected,
                    EventName = "OnConnected",
                    Message = $"ConnectionId '{Context.ConnectionId}' connected to hub '{HubName}'",
                };
                Clients.Group("Events-Connections").OnEvent(model);
            }



            return base.OnConnected();
        }


        public override Task OnReconnected()
        {
            HubServer.Instance.ConnectionManager.OnReconnected(this);



            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            if (client != null)
            {
                var model = new HubEvent
                {
                    HubName = HubName,
                    AgentId = client.Handshake.AgentId,
                    ConnectionId = Context.ConnectionId,
                    Connected = client.Connected,
                    EventName = "OnReconnected",
                    Message = $"ConnectionId '{Context.ConnectionId}' reconnected to hub '{HubName}'",
                };
                Clients.Group("Events-Connections").OnEvent(model);
            }


            return base.OnReconnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            HubServer.Instance.ConnectionManager.OnDisconnected(this, stopCalled);


            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            if (client != null)
            {
                var model = new HubEvent
                {
                    HubName = HubName,
                    AgentId = client.Handshake.AgentId,
                    ConnectionId = Context.ConnectionId,
                    Connected = client.Connected,
                    EventName = "OnDisconnected",
                    Message = $"ConnectionId '{Context.ConnectionId}' disconnected from hub '{HubName}'",
                };
                Clients.Group("Events-Connections").OnEvent(model);
            }


            return base.OnDisconnected(stopCalled);
        }
    }
}
