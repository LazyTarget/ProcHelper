using System;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ServicePluginDescriptor : IServicePlugin
    {
        public ServicePluginDescriptor()
        {

        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public ServiceStatus Status { get; set; }

        public event EventHandler<ServiceStatusChangedEventArgs> OnStatusChanged;

        public Task Init(IExecutionContext context)
        {
            throw new System.NotSupportedException();
        }

        public Task Start()
        {
            throw new System.NotSupportedException();
        }

        public Task Stop()
        {
            throw new System.NotSupportedException();
        }

        public void Dispose()
        {

        }
    }
}
