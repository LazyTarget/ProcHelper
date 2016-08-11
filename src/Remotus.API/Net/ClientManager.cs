using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Remotus.API.Net.Models;
using Remotus.Base;

namespace Remotus.API.Net
{
    public class ClientManager
    {
        private readonly ConcurrentDictionary<string, ConnectedClient> _clients;
        private readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        private TimeSpan _purgeMargin           = TimeSpan.FromMinutes(5);
        private TimeSpan _purgeLoopInterval     = TimeSpan.FromMinutes(1);


        public ClientManager()
        {
            _clients = new ConcurrentDictionary<string, ConnectedClient>();
            ThreadPool.QueueUserWorkItem(PurgeLoop);
        }


        public ConnectedClient GetClient(string connectionId)
        {
            var client = _clients.ContainsKey(connectionId)
                             ? _clients[connectionId]
                             : null;
            return client;
        }

        public IEnumerable<ConnectedClient> GetClients()
        {
            var clients = _clients.Values;
            return clients;
        }


        public void RegisterHub(Hub hub)
        {
            var handshake = hub.Context.Request.GetHandshake();

            var client = GetClient(hub.Context.ConnectionId);
            if (client == null)
            {
                client = new ConnectedClient();
                client.Handshake = handshake;
                client.ConnectionId = hub.Context.ConnectionId;
            }
            client.Connected = true;
            var hubName = hub.GetType().Name;
            var added = client.Hubs.Add(hubName);
            _clients[hub.Context.ConnectionId] = client;
        }


        public void UnregisterHub(Hub hub)
        {
            var client = GetClient(hub.Context.ConnectionId);
            if (client != null)
            {
                var hubName = hub.GetType().Name;
                var removed = client.Hubs.Remove(hubName);
                if (!client.Hubs.Any())
                {
                    //removed = _clients.TryRemove(hub.Context.ConnectionId, out client);
                }
            }
        }


        private void PurgeLoop(object state)
        {
            var now = DateTime.UtcNow;
            var margin = _purgeMargin;
            var connectionIds = _clients
                .Where(x => !x.Value.Connected && x.Value.TimeDisconnected.HasValue &&
                            now.Subtract(margin) > x.Value.TimeDisconnected)
                .Select(x => x.Key)
                .ToArray();
            foreach (var connectionId in connectionIds)
            {
                ConnectedClient client;
                var removed = _clients.TryRemove(connectionId, out client);
                if (removed)
                    _log.Debug(() => $"Client '{connectionId}' has been purged from memory");
                else
                {
                    
                }
            }

            Thread.Sleep(_purgeLoopInterval);
            var count = (state as int?) + 1 ?? 1;
            ThreadPool.QueueUserWorkItem(PurgeLoop, count);
        }

    }
}
