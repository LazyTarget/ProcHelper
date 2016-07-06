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
            
        }

        public void OnReconnected(HubCallerContext context)
        {
            
        }

        public void OnDisconnected(HubCallerContext context, bool stopCalled)
        {
            
        }
    }
}