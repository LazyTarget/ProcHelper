using System;
using System.Collections.Generic;
using System.Linq;
using FullCtrl.API.Config;
using FullCtrl.API.Models;
using FullCtrl.Base;
using Lux.Config.Xml;

namespace FullCtrl.API
{
    public class FullCtrlServer : IDisposable
    {
        private readonly IDictionary<string, IClientInfo> _clients;
        private bool _started;
        private bool _disposed;
        private IServerInfo _serverInfo;

        public FullCtrlServer()
        {
            _clients = new Dictionary<string, IClientInfo>();
        }

        public ServerConfig Config { get; private set; }

        public IServerInfo ServerInfo
        {
            get { return _serverInfo; }
        }


        public void Start()
        {
            if (_started)
                return;
            _started = true;


            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<ServerConfig>();
            //Config = Lux.Framework.ConfigManager.Load<ServerConfig>(descriptor);

            _serverInfo = new ServerInfo
            {
                ApiAddress = Config?.ServerApiAddress,
                ApiVersion = 1,
                InstanceID = Guid.NewGuid().ToString(),
            };
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
