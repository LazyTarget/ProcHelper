using Remotus.Base;

namespace Remotus.API
{
    public class ExecutionContext : IExecutionContext
    {
        public IClientInfo ClientInfo { get; set; }
        public ILog Logger { get; set; }
    }
}
