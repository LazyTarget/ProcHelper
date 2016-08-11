namespace Remotus.Base.Interfaces.Net
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
