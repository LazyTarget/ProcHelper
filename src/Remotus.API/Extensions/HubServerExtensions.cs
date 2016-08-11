using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using Remotus.API.Net;

namespace Remotus.API
{
    public static class HubServerExtensions
    {
        public static T ToGroups<T>(this IHubConnectionContext<T> context, params string[] groupNames)
        {
            if (groupNames == null)
                throw new ArgumentNullException(nameof(groupNames));
            var clients = HubServer.Instance.ClientManager.GetClients().Where(x => x.Connected && x.Groups.Any(groupNames.Contains));
            var connectionIds = clients.Select(x => x.ConnectionId).Distinct().ToArray();
            var result = context.Clients(connectionIds);
            return result;
        }


        public static T ToAgents<T>(this IHubConnectionContext<T> context, params string[] agentIds)
        {
            if (agentIds == null)
                throw new ArgumentNullException(nameof(agentIds));
            var clients = HubServer.Instance.ClientManager.GetClients().Where(x => x.Connected && agentIds.Contains(x.Handshake.AgentId));
            var connectionIds = clients.Select(x => x.ConnectionId).Distinct().ToArray();
            var result = context.Clients(connectionIds);
            return result;
        }

    }
}