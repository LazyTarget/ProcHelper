using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Remotus.Base;
using Remotus.Base.Payloads;

namespace Remotus.API.Net.Hubs
{
    public abstract class HubBase : Hub
    {
        private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);


        public abstract string HubName { get; }


        public async Task AddToGroup(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                return;
            }
            group = group.Trim();



            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            if (!client.Groups.Contains(group))
            {
                client.Groups.Add(group);
                await Groups.Add(Context.ConnectionId, group);


                var model = new HubEvent
                {
                    HubName = HubName,
                    AgentId = client.Handshake.AgentId,
                    ConnectionId = Context.ConnectionId,
                    Connected = client.Connected,
                    EventName = "AddedToGroup",
                    Message = $"ConnectionId '{Context.ConnectionId}' added to group {group} ({HubName})",
                };
                Clients.ToGroups("Events", "Events.Group").OnEvent(model);
            }
            else
            {
                _log.Debug(() => $"Hub has already joined group '{group}'");
            }
        }


        public async Task RemoveFromFroup(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                return;
            }
            group = group.Trim();
            

            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            if (client.Groups.Contains(group))
            {
                var r = client.Groups.Remove(group);
                await Groups.Remove(Context.ConnectionId, group);


                var model = new HubEvent
                {
                    HubName = HubName,
                    AgentId = client.Handshake.AgentId,
                    ConnectionId = Context.ConnectionId,
                    Connected = client.Connected,
                    EventName = "RemovedFromGroup",
                    Message = $"ConnectionId '{Context.ConnectionId}' removed from group {group} ({HubName})",
                };
                Clients.ToGroups("Events", "Events.Group").OnEvent(model);
            }
            else
            {
                _log.Debug(() => $"Cannot remove Hub from group '{group}' as it is currently not joined");
            }
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
                Clients.ToGroups("Events", "Events.Connections").OnEvent(model);
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
                Clients.ToGroups("Event", "Events.Connections").OnEvent(model);
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
                Clients.ToGroups("Events", "Events.Connections").OnEvent(model);
            }


            return base.OnDisconnected(stopCalled);
        }
    }
}
