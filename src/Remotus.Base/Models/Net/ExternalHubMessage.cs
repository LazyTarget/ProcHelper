namespace Remotus.Base.Net
{
    public class ExternalHubMessage : HubMessage
    {
        public ExternalHubMessage()
        {

        }

        //public string AgentId { get; set; }

        public string HubName { get; set; }
    }
}
