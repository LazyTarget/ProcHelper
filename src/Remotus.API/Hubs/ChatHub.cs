using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.API.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {
            
        }

        public void Hello()
        {
            Clients.All.hello();
        }


        public void Send(string name)
        {
            Send(name, "foo bar");
        }

        public void Send(string name, string message)
        {
            var customId = new CustomUserIdProvider();
            var self = customId.GetUserId(Context.Request);
            Clients.User(self).addNewMessageToPage("SYSTEM", "Your message was sent...");

            Clients.All.addNewMessageToPage(name, message);

            if (name == "Time")
            {
                //while (true)
                //{
                //    object data = DateTime.Now.ToString();
                //    string json = JsonConvert.SerializeObject(data);
                //    EventHub.Instance?.Send("Time", "GetTime", json);

                //    Thread.Sleep(500);
                //}
            }

        }

        public override Task OnConnected()
        {
            HubServer.Instance.ConnectionManager.OnConnected(this);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            HubServer.Instance.ConnectionManager.OnReconnected(this);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            HubServer.Instance.ConnectionManager.OnDisconnected(this, stopCalled);
            return base.OnDisconnected(stopCalled);
        }
    }
}