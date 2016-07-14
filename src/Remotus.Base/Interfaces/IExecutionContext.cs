using System;

namespace Remotus.Base
{
    public interface IExecutionContext
    {
        IClientInfo ClientInfo { get; }
        ILog Logger { get; }
        IRemotusAPI Remotus { get; }
        IHubAgentFactory HubAgentFactory { get; }
    }
}
