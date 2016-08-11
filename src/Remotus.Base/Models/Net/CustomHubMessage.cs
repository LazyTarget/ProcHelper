namespace Remotus.Base.Net
{
    public class CustomHubMessage : HubMessage
    {
        public CustomHubMessage()
        {

        }
        
        public string HubName { get; set; }
        public string[] Groups { get; set; }
    }
}
