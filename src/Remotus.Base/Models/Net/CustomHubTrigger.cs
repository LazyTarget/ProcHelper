namespace Remotus.Base
{
    public class CustomHubTrigger : HubTrigger
    {
        public CustomHubTrigger()
        {

        }

        public CustomHubTrigger(string hubName, string eventName, IParameterCollection parameters = null)
            : this()
        {
            HubName = hubName;
            EventName = eventName;
            Parameters = parameters;
        }

        public override string HubName { get; protected set; }
        public override string EventName { get; protected set; }
        public override IParameterCollection Parameters { get; protected set; }
    }
}
