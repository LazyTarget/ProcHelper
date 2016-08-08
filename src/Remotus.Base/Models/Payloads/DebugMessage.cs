namespace Remotus.Base.Payloads
{
    public class DebugMessage
    {
        public DebugMessage()
        {

        }

        public DebugMessage(string agentId, string hubName, string message, int severity)
        {
            AgentId = agentId;
            HubName = hubName;
            Message = message;
            Severity = severity;
        }

        public string AgentId { get; set; }

        public string HubName { get; set; }

        public string Message { get; set; }

        public int Severity { get; set; }
    }
}
