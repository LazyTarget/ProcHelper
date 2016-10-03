namespace Remotus.Base
{
    public abstract class HubTrigger
    {
        public abstract string HubName { get; protected set; }
        public abstract string EventName { get; protected set; }
        public abstract IParameterCollection Parameters { get; protected set; }
    }
}
