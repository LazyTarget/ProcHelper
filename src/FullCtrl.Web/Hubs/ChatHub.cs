using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace FullCtrl.Web.Hubs
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

        
    }
}