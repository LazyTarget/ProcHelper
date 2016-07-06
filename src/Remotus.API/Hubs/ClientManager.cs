using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remotus.API.Hubs
{
    public class ClientManager
    {
        private readonly ConcurrentDictionary<string, Client> _clients;

        public ClientManager()
        {
            _clients = new ConcurrentDictionary<string, Client>();
        }


        public Client GetClient(string connectionId)
        {
            var client = _clients.ContainsKey(connectionId)
                             ? _clients[connectionId]
                             : null;
            return client;
        }

        public void RegisterClient(HubCallerContext context)
        {
            var handshake = context.Request.GetHandshake();

            var client = new Client();
            client.ConnectionId = context.ConnectionId;
            client.Connected = true;
            client.Handshake = handshake;
            _clients[context.ConnectionId] = client;
        }

        public void UnregisterClient(HubCallerContext context)
        {
            Client client;
            var removed = _clients.TryRemove(context.ConnectionId, out client);
        }
    }
}
