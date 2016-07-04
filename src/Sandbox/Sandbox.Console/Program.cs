using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Sandbox.Console
{
    class Program
    {

        private static AutoResetEvent tmr = new AutoResetEvent(false);
        private static HubConnection _connection;
        private static IHubProxy _hubEventProxy;

        static void Main()
        {
            var timeout = TimeSpan.FromMinutes(2);
            Thread.Sleep(timeout);

            timeout = TimeSpan.FromSeconds(30);
            var t = ConnectHub();
            t.Wait(timeout);
            
            tmr.WaitOne();
        }


        static async Task ConnectHub()
        {
            var url = "http://localhost:16196/";
            var connection = new HubConnection(url);

            _hubEventProxy = connection.CreateHubProxy("EventHub");
            var onEventSub = _hubEventProxy.Subscribe("onEvent");
            onEventSub.Received += OnEventSubOnReceived;

            var hubChatProxy = connection.CreateHubProxy("ChatHub");
            var onChatSub = hubChatProxy.Subscribe("addNewMessageToPage");
            onChatSub.Received += OnEventSubOnReceived;

            await connection.Start();
        }

        private static void OnEventSubOnReceived(IList<JToken> list)
        {
            Debug.WriteLine("OnEventSubOnReceived");
            foreach (var tkn in list)
            {
                Debug.WriteLine("OnEventSub: " + tkn);
                if (tkn?.ToString() == "exit")
                    tmr.Set();
                if (tkn?.ToString() == "ping")
                {
                    _hubEventProxy.Invoke("Send", new[] {list[0], "respond", "pong!"});
                }
            }
        }
    }
}
