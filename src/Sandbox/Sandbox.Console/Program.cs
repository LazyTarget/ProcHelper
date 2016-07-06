using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus;
using Remotus.API.Models;
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

            _connection = InitHub();
            ConnectToHub(_connection);

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
                    ConnectToHub(_connection);
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




        static HubConnection InitHub()
        {
            var customJsonSerializerSettings = new CustomJsonSerializerSettings();
            var jsonSerializerSettings = customJsonSerializerSettings.Settings;
            var jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
            jsonSerializer.Formatting = Formatting.None;

            // Hub context
            var url = "http://localhost:9000/signalr";
            var queryString = new Dictionary<string, string>();
            queryString["hub-version"] = "1.0";
            queryString["machine-name"] = Environment.MachineName;
            queryString["username"] = Environment.UserName;
            queryString["UserDomainName"] = Environment.UserDomainName;

            var handshake = new HubHandshake();
            handshake.MachineName = Environment.MachineName;
            handshake.ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            handshake.UserName = Environment.UserName;
            handshake.UserDomainName = Environment.UserDomainName;
            handshake.ClientVersion = "1.0";
            handshake.ClientKey = "qSdhjkZ672zzz";
            handshake.Address = new Uri("http://localhost:9000");

            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            jsonSerializer.Serialize(stringWriter, handshake);
            var handshakeJson = stringBuilder.ToString();

            // Instantiate
            var connection = new HubConnection(url, queryString);
            connection.JsonSerializer = jsonSerializer;
            connection.StateChanged += Connection_OnStateChanged;
            connection.Headers["App-Handshake"] = handshakeJson;

            // Subscriptions
            _hubEventProxy = connection.CreateHubProxy("EventHub");
            var onEventSub = _hubEventProxy.Subscribe("onEvent");
            onEventSub.Received += OnEventSubOnReceived;
            System.Console.WriteLine("Subscribing to EventHub:onEvent");

            var hubChatProxy = connection.CreateHubProxy("ChatHub");
            var onChatSub = hubChatProxy.Subscribe("addNewMessageToPage");
            onChatSub.Received += OnEventSubOnReceived;
            System.Console.WriteLine("Subscribing to ChatHub:addNewMessageToPage");
            return connection;
        }


        static void ConnectToHub(HubConnection connection)
        {
            Task t;
            try
            {
                // Start
                System.Console.WriteLine("Connecting to SignalR server...");
                t = connection.Start();
                t.Wait(millisecondsTimeout: -1);

                // Connected
                if (connection.State == ConnectionState.Connected)
                {
                    System.Console.WriteLine("Connected as: " + connection.ConnectionId);
                    System.Console.WriteLine("Token: " + connection.ConnectionToken);
                }
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine("Error connecting to SignalR server: " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error connecting to SignalR server: " + ex.Message);
                System.Diagnostics.Debug.WriteLine(ex);
            }
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
            if (_connection.State != ConnectionState.Disconnected)
                return;

            ConnectToHub(_connection);
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
