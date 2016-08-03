using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remotus.API.Hubs
{
    public class ClientManager
    {
        private readonly ConcurrentDictionary<string, ConnectedClient> _clients;

        public ClientManager()
        {
            _clients = new ConcurrentDictionary<string, ConnectedClient>();
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
                    removed = _clients.TryRemove(hub.Context.ConnectionId, out client);
                }
            }
        }

    }
}
