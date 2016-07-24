using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Remotus.Base.Models.Payloads;

namespace Remotus.API.Hubs
{
    public class DiagnosticsHub : HubBase
    {
        // Events:
        // OnDebugMessage(DebugMessage model)

        public void ListenTo(string agentId)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, "ListenTo_" + agentId);
        }

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
            //Clients.Group("ListenTo_" + agentId).OnDebugMessage(model);
        }

        public void Write(DebugMessage model, string t)
        {
            var client = HubServer.Instance.ClientManager.GetClient(Context.ConnectionId);
            var agentId = client.Handshake.AgentId;
            Clients.Group("ListenTo_" + agentId).OnDebugMessage(model);
        }
    }
}
