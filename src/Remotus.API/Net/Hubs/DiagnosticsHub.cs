using System;
using System.Reflection;
using Remotus.Base.Payloads;

namespace Remotus.API.Net.Hubs
{
    public class DiagnosticsHub : HubBase
    {
        // Events:
        // OnDebugMessage(DebugMessage model)

        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;


        [Obsolete]
        public void ListenTo(string agentId)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, "ListenTo_" + agentId);
        }

        [Obsolete]
        public void StopListenTo(string agentId)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            Groups.Remove(Context.ConnectionId, "ListenTo_" + agentId);
        }


        public void ConfigureListener(string agentId, int severity)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);

            if (severity <= 0)
            {
                StopListenTo(agentId);
            }
            else
            {
                ListenTo(agentId);
            }
        }


        public void Write(string json)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            var agentId = client.Handshake.AgentId;
            var model = new DebugMessage(agentId, HubName, json, 0);
            Clients.Group("ListenTo_" + agentId).OnDebugMessage(model);
        }

        public void Write(DebugMessage model)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            var agentId = client.Handshake.AgentId;
            Clients.Group("ListenTo_" + agentId).OnDebugMessage(model);
        }

        public void Write(DebugMessage model, string t)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            var agentId = client.Handshake.AgentId;
            Clients.Group("ListenTo_" + agentId).OnDebugMessage(model);
        }

    }
}
