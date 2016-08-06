using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Remotus.API.Hubs
{
    public class ChatHub : HubBase
    {
        public ChatHub()
        {

        }

        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;


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
            Clients.All.addNewMessageToPage(name, message);


            var customId = new CustomUserIdProvider();
            var self = customId.GetUserId(Context.Request);
            if (!string.IsNullOrWhiteSpace(self))
            {
                Clients.User(self).addNewMessageToPage("SYSTEM", "Your message was sent... (user)");
            }
            Clients.Client(Context.ConnectionId).addNewMessageToPage("SYSTEM", "Your message was sent... (client)");
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

    }
}