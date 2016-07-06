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


        public void Send(string name, string message)
        {
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
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}