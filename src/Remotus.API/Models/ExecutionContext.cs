using Remotus.Base;

namespace Remotus.API
{
    public class ExecutionContext : IExecutionContext
    {
        public IClientInfo ClientInfo { get; set; }
        public ILog Logger { get; set; }
        public IRemotusAPI Remotus { get; set; }
        public IHubAgentFactory HubAgentFactory { get; set; }
    }
}
