using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Remotus.API.Hubs
{
    public class ConnectionManager
    {
        private readonly ClientManager _clientManager;
        private readonly object _locker;

        public ConnectionManager(ClientManager clientManager, object locker)
        {
            _clientManager = clientManager;
            _locker = locker;
        }


        public void OnConnected(Hub hub)
        {
            lock (_locker)
            {
                _clientManager.RegisterHub(hub);

                var client = _clientManager.GetClient(hub.Context.ConnectionId);
                if (client != null)
                {
                    client.Connected = true;
                }
            }
        }

        public void OnReconnected(Hub hub)
        {
            lock (_locker)
            {
                _clientManager.RegisterHub(hub);

                var client = _clientManager.GetClient(hub.Context.ConnectionId);
                if (client != null)
                {
                    client.Connected = true;
                }
            }
        }

        public void OnDisconnected(Hub hub, bool stopCalled)
        {
            lock (_locker)
            {
                _clientManager.UnregisterHub(hub);

                var client = _clientManager.GetClient(hub.Context.ConnectionId);
                if (client != null)
                {
                    client.Connected = false;
                }
            }
        }

    }
}