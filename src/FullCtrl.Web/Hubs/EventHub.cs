using Microsoft.AspNet.SignalR;

namespace FullCtrl.Web.Hubs
{
    public class EventHub : Hub
    {
        public static EventHub Instance;

        public EventHub()
        {
            Instance = this;
        }


        public void Send(string channelName, string eventName, string json)
        {
            Clients.All.onEvent(channelName, eventName, json);
        }
    }
}