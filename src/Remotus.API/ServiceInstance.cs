using System;
using Lux.Config.Xml;

namespace Remotus.API
{
    internal class ServiceInstance : IDisposable
    {
        public FullCtrlServer Server { get; private set; }
        public FullCtrlClient Client { get; private set; }

        public ServiceInstance()
        {
            
        }
        
        protected virtual ServerConfig LoadServerConfig()
        {
            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<ServerConfig>();
            var config = Lux.Framework.ConfigManager.Load<ServerConfig>(descriptor);
            return config;
        }

        protected virtual ClientConfig LoadClientConfig()
        {
            var descriptorFactory = new AppConfigDescriptorFactory();
            var descriptor = descriptorFactory.CreateDescriptor<ClientConfig>();
            var config = Lux.Framework.ConfigManager.Load<ClientConfig>(descriptor);
            return config;
        }


        public void Start(string[] args)
        {
            Server = Server ?? new FullCtrlServer();
            Server.Start(LoadServerConfig());

            Client = Client ?? new FullCtrlClient();
            Client.Start(LoadClientConfig());
        }


        public void Stop()
        {
            Client?.Stop();

            Server?.Stop();
        }

        public void Dispose()
        {
            Stop();
        }

    }
}
