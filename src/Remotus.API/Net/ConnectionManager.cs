using System;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Remotus.Base;

namespace Remotus.API.Net
{
    public class ConnectionManager
    {
        private readonly ClientManager _clientManager;
        private readonly object _locker;
        private readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        public ConnectionManager(ClientManager clientManager, object locker)
        {
            _clientManager = clientManager;
            _locker = locker;
        }


        public void OnConnected(Hub hub)
        {
            lock (_locker)
            {
                _log.Debug(() => $"OnConnected() {hub.Context.ConnectionId} to hub '{hub.GetType().Name}'");

                _clientManager.RegisterHub(hub);

                var client = _clientManager.GetClient(hub.Context.ConnectionId);
                if (client != null)
                {
                    client.Connected = true;
                    client.TimeDisconnected = null;
                }
            }
        }

        public void OnReconnected(Hub hub)
        {
            lock (_locker)
            {
                _log.Debug(() => $"OnReconnected() {hub.Context.ConnectionId} to hub '{hub.GetType().Name}'");

                _clientManager.RegisterHub(hub);

                var client = _clientManager.GetClient(hub.Context.ConnectionId);
                if (client != null)
                {
                    client.Connected = true;
                    client.TimeDisconnected = null;
                }
            }
        }

        public void OnDisconnected(Hub hub, bool stopCalled)
        {
            lock (_locker)
            {
                _log.Debug(() => $"OnDisconnected() {hub.Context.ConnectionId} from hub '{hub.GetType().Name}'");

                _clientManager.UnregisterHub(hub);

                var client = _clientManager.GetClient(hub.Context.ConnectionId);
                if (client != null)
                {
                    client.Connected = false;
                    client.TimeDisconnected = DateTime.UtcNow;
                }
            }
        }

    }
}