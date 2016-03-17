using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;
using Remotus.Base;

namespace Remotus.API
{
    public class FullCtrlServer : IDisposable
    {
        private readonly IDictionary<string, IClientInfo> _clients;
        private bool _started;
        private bool _disposed;
        private IServerInfo _serverInfo;
        private IDisposable _server;

        public FullCtrlServer()
        {
            _clients = new Dictionary<string, IClientInfo>();
        }
        
        public IServerInfo ServerInfo
        {
            get { return _serverInfo; }
        }

        
        protected virtual StartOptions GetStartOptions(ServerConfig config)
        {
            var options = new StartOptions();
            options.Urls.Add("http://+:9000");
            options.Urls.Add("http://localhost:9000");
            return options;
        }

        protected virtual IDisposable StartAsSelftHost(StartOptions options)
        {
            var res = WebApp.Start<StartupServer>(options);
            return res;
        }


        public virtual void Start(ServerConfig config)
        {
            if (_started)
                return;
            _started = true;

            
            // Info
            _serverInfo = new ServerInfo
            {
                ApiAddress = config?.ServerApiAddress,
                ApiVersion = 1,
                InstanceID = Guid.NewGuid().ToString(),
            };



            // Start WebAPI
            Console.WriteLine("Server starting");

            var options = GetStartOptions(config);
            _server = StartAsSelftHost(options);

            Console.WriteLine("Server started");
        }

        public void Stop()
        {
            _started = false;


            // Stop WebAPI
            if (_server != null)
            {
                Console.WriteLine("Server stopping");
                _server.Dispose();
                _server = null;
                Console.WriteLine("Server stopped");
            }

            // Clients
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
            _disposed = true;
            Stop();
            _clients.Clear();
        }
    }
}
