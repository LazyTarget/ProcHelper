using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus;

namespace Sandbox.Console
{
    class Program
    {
        private static IHubProxy _hubEventProxy;

        static void Main()
        {
            //var timeout = TimeSpan.FromMinutes(1);
            //Thread.Sleep(timeout);

            Task<HubConnection> t = null;
            while (t == null || t.IsFaulted || !t.IsCompleted || t.Result == null)
            {
                try
                {
                    System.Console.WriteLine("Trying to connect to SignalR server");
                    t = ConnectHub();
                    t.Wait(millisecondsTimeout: -1);

                    if (t.IsCompleted && t.Result != null)
                    {
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error connecting to SignalR server");
                    System.Console.WriteLine(ex);
                }
            }

            var connection = t.Result;

            while (true)
            {
                var input = System.Console.ReadLine();
                if (input == "cls")
                {
                    System.Console.Clear();
                }
                else if (input == "discover")
                {
                    _hubEventProxy.Invoke("Discover");
                }
                else if (input == "start")
                {
                    if (connection.State != ConnectionState.Disconnected)
                    {
                        System.Console.WriteLine("Connection is already Connected/Connecting/Reconnecting");
                    }
                    else
                    {
                        t = ConnectHub(connection);
                        t.Wait(millisecondsTimeout: -1);
                        connection = t.Result;
                    }

                    //connection.Stop();
                    connection.Stop(new Exception("This is an 'fake' exception from the client"));
                }
                else if (input == "stop" || input == "exit")
                {
                    //connection.Stop();
                    connection.Stop(new Exception("This is an 'fake' exception from the client"));
                }


                if (input == "exit")
                    break;
            }
        }


        static async Task<HubConnection> ConnectHub(HubConnection connection = null)
        {
            if (connection == null)
            {
                var customJsonSerializerSettings = new CustomJsonSerializerSettings();
                var jsonSerializerSettings = customJsonSerializerSettings.Settings;

                // Hub context
                var url = "http://localhost:9000/signalr";
                var queryString = new Dictionary<string, string>();
                queryString["hub-version"] = "1.0";
                queryString["machine-name"] = Environment.MachineName;
                queryString["username"] = Environment.UserName;
                queryString["UserDomainName"] = Environment.UserDomainName;

                // Instantiate
                connection = new HubConnection(url, queryString);
                connection.JsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
                connection.StateChanged += Connection_OnStateChanged;

                // Subscriptions
                _hubEventProxy = connection.CreateHubProxy("EventHub");
                var onEventSub = _hubEventProxy.Subscribe("onEvent");
                onEventSub.Received += OnEventSubOnReceived;
                System.Console.WriteLine("Subscribing to EventHub:onEvent");

                var hubChatProxy = connection.CreateHubProxy("ChatHub");
                var onChatSub = hubChatProxy.Subscribe("addNewMessageToPage");
                onChatSub.Received += OnEventSubOnReceived;
                System.Console.WriteLine("Subscribing to ChatHub:addNewMessageToPage");
            }

            // Start
            System.Console.WriteLine("Connecting to SignalR server...");
            await connection.Start();

            // Started
            System.Console.WriteLine("Connected as: " + connection.ConnectionId);
            System.Console.WriteLine("Token: " + connection.ConnectionToken);
            return connection;
        }

        private static void Connection_OnStateChanged(StateChange stateChange)
        {
            System.Console.WriteLine("Connection changed: {0} => {1}", stateChange.OldState, stateChange.NewState);
        }


        private static void OnEventSubOnReceived(IList<JToken> list)
        {
            System.Console.WriteLine("OnEventSubOnReceived");
            foreach (var tkn in list)
            {
                System.Console.WriteLine("OnEventSub: " + tkn);
                if (tkn?.ToString() == "ping")
                {
                    //_hubEventProxy.Invoke("Send", new[] {list[0], "respond", "pong!"});
                }
            }
        }
    }
}
