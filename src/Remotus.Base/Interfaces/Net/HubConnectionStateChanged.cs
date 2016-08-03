namespace Remotus.Base.Net
{
    public class HubConnectionStateChange
    {
        public HubConnectionStateChange(ConnectionState oldState, ConnectionState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public ConnectionState OldState { get; }
        public ConnectionState NewState { get; }
    }
}
