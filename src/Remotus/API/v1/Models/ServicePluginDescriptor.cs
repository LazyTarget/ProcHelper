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

        public void Init(IExecutionContext context)
        {
            throw new System.NotSupportedException();
        }

        public void Start()
        {
            throw new System.NotSupportedException();
        }

        public void Stop()
        {
            throw new System.NotSupportedException();
        }

        public void Dispose()
        {

        }
    }
}
