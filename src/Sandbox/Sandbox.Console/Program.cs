using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Fclp;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus;
using Remotus.API.Models;
using Timer = System.Timers.Timer;

namespace Sandbox.Console
{
    static class Program
    {
        private static HubConnection _connection;
        private static IHubProxy _hubEventProxy;
        private static readonly Timer _reconnectTimer;
        private static ReconnectArguments _reconnectArgs;


        static Program()
        {
            _reconnectArgs = new ReconnectArguments();
            _reconnectTimer.AutoReset = true;
            _reconnectTimer.Enabled = true;
            _reconnectTimer.Interval = _reconnectArgs.ReconnectInterval.TotalMilliseconds;
            _reconnectTimer.Elapsed += ReconnectTimer_OnElapsed;
        }

        
        static void Main(string[] args)
        {
            RunAsConsole();
        }

        
        private static void RunAsConsole()
        {
            while (true)
            {
                var input = System.Console.ReadLine();
                var verb = input;
                try
                {
                    if (input == "cls")
                    {
                        System.Console.Clear();
                    }
                    else if (input == "help")
                    {
                        PrintHelp();
                    }
                    else if (input == "exit")
                    {
                        if (_connection != null)
                            DisconnectHub();
                    }


                    var args = new string[0];
                    if (!string.IsNullOrWhiteSpace(input))
                        args = input.Split(' ');
                    verb = args.Length > 0 ? args[0] : null;
                    args = args.Length > 1 ? args.Skip(1).ToArray() : args;

                    var a = new CommandArguments
                    {
                        Verb = verb,
                        Arguments = args,
                    };
                    ProcessInput(a);
                }
                catch (AggregateException ex)
                {
                    System.Console.WriteLine($"Error executing command '{verb}': {ex.InnerException.Message}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error executing command '{verb}': {ex.InnerException.Message}");
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }


        private static void PrintHelp()
        {
            System.Console.WriteLine("Commands: ");
            System.Console.WriteLine("* discover");
            System.Console.WriteLine("* connect");
            System.Console.WriteLine("* reconnect");
            System.Console.WriteLine("* disconnect");
            System.Console.WriteLine("* subscribe");
            System.Console.WriteLine("* unsubscribe");
            System.Console.WriteLine("* send");
        }


        private static void ProcessInput(CommandArguments a)
        {
            var verb = a.Verb;
            if (verb == "discover")
            {
                //_hubEventProxy.Invoke("Discover");
            }
            else if (verb == "connect" ||
                     verb == "connect-hub")
            {
                VerbConnectHub(a);
            }
            else if (verb == "reconnect")
            {
                VerbReconnectHub(a);
            }
            else if (verb == "disconnect")
            {
                VerbDisconnectHub(a);
            }
            else if (verb == "subscribe")
            {

            }
            else if (verb == "unsubscribe")
            {

            }
            else if (verb == "send")
            {
                //var text = input.Substring(0, "say ".Length);
                //_hubEventProxy.Invoke("Send", new[] { text });
            }
        }




        private static void VerbConnectHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<ConnectArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.Host)
                .As('h', "host")
                .SetDefault("localhost")
                .WithDescription("Enter the host address to the SignalR server");
            p.Setup(x => x.Port)
                .As('p', "port")
                .SetDefault(9000)
                .WithDescription("Enter the port to the SignalR server");
            p.SetupHelp("help");

            var args = a.Verb == "connect"
                ? a.Arguments
                : new string[0];
            var r = p.Parse(args);
            if (r.HasErrors)
            {
                System.Console.WriteLine("Error: " + r.ErrorText);
            }
            else if (r.HelpCalled)
            {
                System.Console.WriteLine($"Help for {a.Verb} not implemented...");
            }
            else
            {
                ConnectHub(p.Object);
            }
        }

        
        private static void ConnectHub(ConnectArguments arguments = null)
        {
            arguments = arguments ?? new ConnectArguments();

            var url = $"http://{arguments.Host}:{arguments.Port}/signalr";

            if (_connection != null)
            {
                if (_connection.Url == url)
                {
                    System.Console.WriteLine($"Already connected to specified server... ({_connection.State})");
                    if (_connection.State == ConnectionState.Disconnected)
                    {
                        System.Console.WriteLine("State is Disconnected. Will try to reconnect...");
                        ReconnectHub();
                    }
                    return;
                }
                else
                {
                    System.Console.WriteLine("Already connected to another server. You must disconnect before you can connect to another server.");
                    return;
                }
            }


            var customJsonSerializerSettings = new CustomJsonSerializerSettings();
            var jsonSerializerSettings = customJsonSerializerSettings.Settings;
            var jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
            jsonSerializer.Formatting = Formatting.None;
                

            var handshake = new HubHandshake();
            handshake.MachineName = Environment.MachineName;
            handshake.ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            handshake.UserName = Environment.UserName;
            handshake.UserDomainName = Environment.UserDomainName;
            handshake.ClientVersion = "1.0";
            handshake.ClientKey = "qSdhjkZ672zzz";
            handshake.Address = new Uri("http://localhost:9000");

            // Hub context
            var queryString = new Dictionary<string, string>();
            queryString["hub-version"] = "1.0";
            queryString["machine-name"] = Environment.MachineName;
            queryString["username"] = Environment.UserName;
            queryString["UserDomainName"] = Environment.UserDomainName;

            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            jsonSerializer.Serialize(stringWriter, handshake);
            var handshakeJson = stringBuilder.ToString();

            // Instantiate
            var connection = new HubConnection(url, queryString);
            connection.JsonSerializer = jsonSerializer;
            connection.StateChanged += Connection_OnStateChanged;
            connection.Headers["App-Handshake"] = handshakeJson;

            //// Subscriptions
            //_hubEventProxy = connection.CreateHubProxy("EventHub");
            //var onEventSub = _hubEventProxy.Subscribe("onEvent");
            //onEventSub.Received += OnEventSubOnReceived;
            //System.Console.WriteLine("Subscribing to EventHub:onEvent");

            //var hubChatProxy = connection.CreateHubProxy("ChatHub");
            //var onChatSub = hubChatProxy.Subscribe("addNewMessageToPage");
            //onChatSub.Received += OnEventSubOnReceived;
            //System.Console.WriteLine("Subscribing to ChatHub:addNewMessageToPage");


            System.Console.WriteLine("Connecting to SignalR server...");
            var connectTimeout = _reconnectArgs?.ConnectTimeout ?? TimeSpan.FromSeconds(30);
            connection.Start().Wait(connectTimeout);
            if (connection.State == ConnectionState.Connected)
            {
                System.Console.WriteLine("Connected as: " + connection.ConnectionId);
                System.Console.WriteLine("Token: " + connection.ConnectionToken);
            }

            _connection = connection;
        }



        private static void VerbReconnectHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<ReconnectArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.AutoReconnect)
                .As('a', "auto")
                .SetDefault(false)
                .WithDescription("Whether to automatically reconnect");
            p.Setup(x => x.ReconnectInterval)
                .As('t')
                .SetDefault(TimeSpan.FromSeconds(3))
                .WithDescription("The duration between reconnect tries");
            p.SetupHelp("help");

            var args = a.Verb == "reconnect"
                ? a.Arguments
                : new string[0];
            var r = p.Parse(args);
            if (r.HasErrors)
            {
                System.Console.WriteLine("Error: " + r.ErrorText);
            }
            else if (r.HelpCalled)
            {
                System.Console.WriteLine($"Help for {a.Verb} not implemented...");
            }
            else
            {
                _reconnectArgs = p.Object;
                ReconnectHub(_reconnectArgs);
            }
        }


        private static void ReconnectHub(ReconnectArguments arguments = null)
        {
            if (_connection == null)
            {
                _reconnectTimer.Stop();
                System.Console.WriteLine("No server to reconnect to...");
                return;
            }
            if (_connection.State == ConnectionState.Connected)
            {
                _reconnectTimer.Stop();
                return;
            }
            if (_connection.State != ConnectionState.Disconnected)
                return;

            arguments = arguments ?? new ReconnectArguments();
            if (arguments.AutoReconnect)
            {
                _reconnectTimer.Stop();
                _reconnectTimer.Enabled = true;
                _reconnectTimer.AutoReset = true;
                _reconnectTimer.Interval = arguments.ReconnectInterval.TotalMilliseconds;
                _reconnectTimer.Start();
            }
            else
            {
                System.Console.WriteLine("Re-connecting to SignalR server...");
                _connection.Start().Wait(arguments.ConnectTimeout);
            }
        }



        private static void VerbDisconnectHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<DisconnectArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.Force)
                .As('f', "force")
                .SetDefault(false)
                .WithDescription("Whether to forcibly close the connection");
            p.Setup(x => x.ForceMessage)
                .As("msg")
                .SetDefault("The connection was forcibly closed by the client")
                .WithDescription("The message to be sent when closing the connection");
            p.SetupHelp("help");

            var args = a.Verb == "disconnect"
                ? a.Arguments
                : new string[0];
            var r = p.Parse(args);
            if (r.HasErrors)
            {
                System.Console.WriteLine("Error: " + r.ErrorText);
            }
            else if (r.HelpCalled)
            {
                System.Console.WriteLine($"Help for {a.Verb} not implemented...");
            }
            else
            {
                DisconnectHub(p.Object);
            }
        }
        

        private static void DisconnectHub(DisconnectArguments arguments = null)
        {
            if (_connection == null)
            {
                System.Console.WriteLine("No server connected...");
                return;
            }

            arguments = arguments ?? new DisconnectArguments();
            if (arguments.Force)
            {
                var msg = arguments.ForceMessage;
                var error = new OperationCanceledException(msg);
                _connection.Stop(error);
            }
            else
            {
                _connection.Stop();
            }
                
            _connection.Dispose();
            _connection = null;
        }
        
        

        private static void Connection_OnStateChanged(StateChange stateChange)
        {
            System.Console.WriteLine("Connection changed: {0} => {1}", stateChange.OldState, stateChange.NewState);

            if (stateChange.NewState == ConnectionState.Disconnected)
            {
                if (_reconnectArgs == null || _reconnectArgs.AutoReconnect)
                    _reconnectTimer.Start();
            }
        }


        private static void ReconnectTimer_OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                var arguments = new ReconnectArguments();
                if (_reconnectArgs != null)
                {
                    arguments.ConnectTimeout = _reconnectArgs.ConnectTimeout;
                    arguments.ReconnectInterval = _reconnectArgs.ReconnectInterval;
                }
                arguments.AutoReconnect = false;

                ReconnectHub(arguments);
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine("Error re-connecting to SignalR server: " + ex.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(ex);
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
