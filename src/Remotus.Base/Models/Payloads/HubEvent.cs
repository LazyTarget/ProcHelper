namespace Remotus.Base.Payloads
{
    public class HubEvent
    {
        public HubEvent()
        {

        }
        
        public string EventName { get; set; }

        public string AgentId { get; set; }

        public string ConnectionId { get; set; }

        public string HubName { get; set; }

        public bool Connected { get; set; }

        public string Message { get; set; }
    }
}
