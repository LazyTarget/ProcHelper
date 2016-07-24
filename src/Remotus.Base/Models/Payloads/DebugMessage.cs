namespace Remotus.Base.Models.Payloads
{
    public class DebugMessage
    {
        public DebugMessage(string agentId, string hubName, string message, int severity)
        {
            AgentId = agentId;
            HubName = hubName;
            Message = message;
            Severity = severity;
        }

        public string AgentId { get; private set; }

        public string HubName { get; private set; }

        public string Message { get; private set; }

        public int Severity { get; private set; }
    }
}
