using System;
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


        public void OnConnected(HubCallerContext context)
        {
            lock (_locker)
            {
                var client = _clientManager.GetClient(context.ConnectionId);
                if (client == null)
                {
                    _clientManager.RegisterClient(context);
                }
            }
        }

        public void OnReconnected(HubCallerContext context)
        {
            lock (_locker)
            {
                var client = _clientManager.GetClient(context.ConnectionId);
                if (client == null)
                {
                    _clientManager.RegisterClient(context);
                }
            }
        }

        public void OnDisconnected(HubCallerContext context, bool stopCalled)
        {
            lock (_locker)
            {
                _clientManager.UnregisterClient(context);
            }
        }

    }
}