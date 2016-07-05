using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus;
using Timer = System.Timers.Timer;

namespace Sandbox.Console
{
    class Program
    {
        private static HubConnection _connection;
        private static IHubProxy _hubEventProxy;
        private static readonly Timer _reconnectTimer = new Timer(5000);

        static void Main()
        {
            _reconnectTimer.AutoReset = true;
            _reconnectTimer.Enabled = true;
            _reconnectTimer.Elapsed += ReconnectTimer_OnElapsed;
            
            //var timeout = TimeSpan.FromMinutes(1);
            //Thread.Sleep(timeout);

            Task t = null;
            while (t == null || t.IsFaulted || !t.IsCompleted || _connection == null)
            {
                try
                {
                    System.Console.WriteLine("Trying to connect to SignalR server");
                    t = ConnectHub();
                    t.Wait(millisecondsTimeout: -1);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error connecting to SignalR server: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
            

            while (true)
            {
                var input = System.Console.ReadLine();
                if (input == "cls")
                {
                    System.Console.Clear();
                }
                else if (input?.ToLower().StartsWith("say ") ?? false)
                {
                    var text = input.Substring(0, "say ".Length);
                    _hubEventProxy.Invoke("Send", new [] { text });
                }
                else if (input == "discover")
                {
                    _hubEventProxy.Invoke("Discover");
                }
                else if (input == "auto reconnect")
                {
                    _reconnectTimer.Enabled = true;
                    _reconnectTimer.Start();
                }
                else if (input == "manual reconnect")
                {
                    _reconnectTimer.Enabled = false;
                    _reconnectTimer.Stop();
                }
                else if (input == "start")
                {
                    try
                    {
                        System.Console.WriteLine("Trying to connect to SignalR server");
                        t = ConnectHub();
                        t.Wait(millisecondsTimeout: -1);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error connecting to SignalR server: " + ex.Message);
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
                else if (input == "stop")
                {
                    _connection.Stop();
                }
                else if (input == "force stop" || input == "exit")
                {
                    _connection.Stop(new Exception("This is an 'fake' exception from the client"));
                }


                if (input == "exit")
                    break;
            }
        }


        static async Task<HubConnection> ConnectHub()
        {
            if (_connection == null)
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
                _connection = new HubConnection(url, queryString);
                _connection.JsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
                _connection.StateChanged += Connection_OnStateChanged;

                // Subscriptions
                _hubEventProxy = _connection.CreateHubProxy("EventHub");
                var onEventSub = _hubEventProxy.Subscribe("onEvent");
                onEventSub.Received += OnEventSubOnReceived;
                System.Console.WriteLine("Subscribing to EventHub:onEvent");

                var hubChatProxy = _connection.CreateHubProxy("ChatHub");
                var onChatSub = hubChatProxy.Subscribe("addNewMessageToPage");
                onChatSub.Received += OnEventSubOnReceived;
                System.Console.WriteLine("Subscribing to ChatHub:addNewMessageToPage");
            }

            // Start
            System.Console.WriteLine("Connecting to SignalR server...");
            await _connection.Start();

            // Connected
            if (_connection.State == ConnectionState.Connected)
            {
                System.Console.WriteLine("Connected as: " + _connection.ConnectionId);
                System.Console.WriteLine("Token: " + _connection.ConnectionToken);
            }
            return _connection;
        }


        private static void Connection_OnStateChanged(StateChange stateChange)
        {
            System.Console.WriteLine("Connection changed: {0} => {1}", stateChange.OldState, stateChange.NewState);

            if (stateChange.NewState == ConnectionState.Disconnected)
            {
                if (_reconnectTimer.Enabled)
                    _reconnectTimer.Start();
            }
        }

        private static void ReconnectTimer_OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_connection == null || _connection.State == ConnectionState.Connected)
            {
                _reconnectTimer.Stop();
                return;
            }

            Task t;
            try
            {
                System.Console.WriteLine("Trying to re-connect to SignalR server");
                t = ConnectHub();
                t.Wait(millisecondsTimeout: -1);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error re-connecting to SignalR server: " + ex.Message);
                System.Diagnostics.Debug.WriteLine(ex);
            }
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
