using System;
using System.Collections.Generic;
using System.Linq;
using FullCtrl.Base;

namespace FullCtrl.API
{
    public class FullCtrlServer : IDisposable
    {
        private readonly IDictionary<string, IClientInfo> _clients;
        private bool _started;
        private bool _disposed;

        public FullCtrlServer()
        {
            _clients = new Dictionary<string, IClientInfo>();
        }


        public void Start()
        {
            if (_started)
                return;
            _started = true;
        }

        public void Stop()
        {
            _started = false;
            _clients.Clear();
        }


        public IEnumerable<IClientInfo> GetClients()
        {
            return _clients.Values;
        } 

        public int RegisterClient(IClientInfo clientInfo)
        {
            if (clientInfo == null)
                throw new ArgumentNullException(nameof(clientInfo));
            if (string.IsNullOrWhiteSpace(clientInfo?.ClientID))
                throw new ArgumentException(nameof(clientInfo));

            _clients[clientInfo.ClientID] = clientInfo;
            return 1;
        }


        public void Dispose()
        {
            if (_disposed)
                return;
            Stop();
            _clients.Clear();
            _disposed = true;
        }
    }
}
