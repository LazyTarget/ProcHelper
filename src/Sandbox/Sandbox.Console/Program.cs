using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using Fclp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus;
using Remotus.API.Hubs.Client;
using Remotus.API.Models;
using Remotus.Base;
using Remotus.Base.Models.Hub;
using Remotus.Base.Net;

namespace Sandbox.Console
{
    static class Program
    {
        private static IHubAgentFactory _hubAgentFactory = new HubAgentFactory();
        //private static readonly IDictionary<string, IHubAgent> _hubs = new Dictionary<string, IHubAgent>();
        private static IHubAgentManager _hubAgentManager;
        private static ReconnectArguments _reconnectArgs;
        private static ZeroconfService.NetServiceBrowser _zeroconfBrowser;


        static Program()
        {
            LogManager.ConfigureLog4Net();
            LogManager.InitializeWith<Log4NetLogger>();

            _reconnectArgs = new ReconnectArguments();
        }


        static void Main(string[] args)
        {
            RunAsConsole(args);


            if (_hubAgentManager?.Connector != null)
            {
                _hubAgentManager.Connector.Disconnect();
                _hubAgentManager.Connector.Dispose();
            }
            if (_hubAgentManager != null)
            {
                _hubAgentManager.Dispose();
                _hubAgentManager = null;
            }
        }


        private static void RunAsConsole(string[] args)
        {
            System.Console.WriteLine("Remotus Sandbox v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            System.Console.WriteLine("For help enter \"help\"");

            Queue<string> buffer = null;
            var firstArg = args.FirstOrDefault();
            if (firstArg == "buffer")
                buffer = new Queue<string>(args.Skip(1));
            while (true)
            {
                string input = null;
                if (buffer?.Count > 0)
                    input = buffer.Dequeue() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    System.Console.Write("> ");
                    input = System.Console.ReadLine();
                }
                else
                    System.Console.WriteLine("> " + input);

                if (string.IsNullOrWhiteSpace(input))
                {
                    System.Console.WriteLine("Invalid input!");
                    continue;
                }

                if (input == "exit")
                {
                    break;
                }
                ProcessInput(input);
            }
        }
        

        private static void ProcessInput(string input)
        {
            var verb = input;
            try
            {
                if (input == "cls")
                {
                    System.Console.Clear();
                }
                else if (input == "help" || input == "-?")
                {
                    PrintHelp();
                }
                else if (input == "exit")
                {
                    return;
                }


                var args = new string[0];
                if (!string.IsNullOrWhiteSpace(input))
                    args = input.Split(' ');
                var switchIndex = args.ToList().FindIndex(x => x.StartsWith("/"));
                if (switchIndex < 0)
                    switchIndex = args.Length;
                verb = switchIndex > 0
                    ? String.Join(" ", args.Take(switchIndex))
                    : args.Length > 0 ? args[0] : null;
                args = switchIndex > 0
                    ? args.Skip(switchIndex).ToArray()
                    : args.Length > 1 ? args.Skip(1).ToArray() : args;

                var a = new CommandArguments
                {
                    Verb = verb,
                    Arguments = args,
                };
                ProcessCommand(a);
            }
            catch (AggregateException ex)
            {
                var msg = ex.GetBaseException().Message;
                System.Console.WriteLine($"Error executing command '{verb}': {msg}");
                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                var msg = ex.GetBaseException().Message;
                System.Console.WriteLine($"Error executing command '{verb}': {msg}");
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }


        private static void ProcessCommand(CommandArguments a)
        {
            var verb = a.Verb;
            if (verb == "discover")
            {
                //_hubEventProxy.Invoke("Discover");
            }
            if (verb == "zeroconf-resolve" ||
                verb == "zeroconf resolve")
            {
                VerbZeroConfResolve(a);
            }
            if (verb == "zeroconf-publish" ||
                verb == "zeroconf publish")
            {
                VerbZeroConfPublish(a);
            }
            else if (verb == "connect")
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
                VerbSubscribeHub(a);
            }
            else if (verb == "unsubscribe")
            {
                VerbUnsubscribeHub(a);
            }
            else if (verb == "send")
            {
                VerbSendToHub(a);
            }
            else
            {
                CommandNotFound(a);
            }
        }




        private static void PrintHelp()
        {
            System.Console.WriteLine("Commands: ");
            //System.Console.WriteLine("* discover");
            System.Console.WriteLine("* zeroconf");
            System.Console.WriteLine("* connect - Connect to hub server");
            System.Console.WriteLine("* reconnect - Reconnect to hub server");
            System.Console.WriteLine("* disconnect - Disconnnect from hub server");
            System.Console.WriteLine("* subscribe - Subscribe to a hub event");
            System.Console.WriteLine("* unsubscribe - Unsubscribe from a hub event");
            System.Console.WriteLine("* send - Invoke a method on a hub");
        }


        private static void CommandNotFound(CommandArguments a)
        {
            if (a.Arguments?.Length > 0)
                System.Console.WriteLine($"Command '{a.Verb}' was not found. Args: {string.Join(" ", a.Arguments)}");
            else
                System.Console.WriteLine($"Command '{a.Verb}' was not found.");
        }


        private static void VerbConnectHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<ConnectArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.Host)
                .As('h', "host")
                //.SetDefault("localhost")
                .WithDescription("Enter the host address to the SignalR server");
            p.Setup(x => x.Port)
                .As('p', "port")
                //.SetDefault(9000)
                .WithDescription("Enter the port to the SignalR server");
            p.Setup(x => x.Hubs)
                .As("hubs")
                .Required()
                .WithDescription("Enter the hubs to connect to. Seperate with commas or spaces");
            p.SetupHelp("help");

            var args = a.Verb == "connect" ||
                       a.Verb == "connect-hub"
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
            if (arguments.Hubs == null)
                throw new ArgumentNullException(nameof(arguments.Hubs));

            var url = $"http://{arguments.Host}:{arguments.Port}/signalr";

            if (_hubAgentManager != null)
            {
                //if (_hubs.Connection?.Url == url)
                //{
                //    if (_hubs.Connection.State == ConnectionState.Disconnected)
                //    {
                //        System.Console.WriteLine("State is Disconnected. Will try to reconnect...");
                //        ReconnectHub();
                //    }
                //    else
                //        System.Console.WriteLine($"Already connected to specified server... ({_hubs.Connection.State})");
                //    return;
                //}
                //else
                //{
                    System.Console.WriteLine("Already connected to another server. You must disconnect before you can connect to another server.");
                    return;
                //}
            }

            var f = _hubAgentFactory as HubAgentFactory;
            if (f != null)
            {
                f.Host = arguments.Host;
                f.Port = arguments.Port;
            }

            ICredentials credentials;
            credentials = CredentialCache.DefaultNetworkCredentials;
            credentials = CredentialCache.DefaultCredentials;
            credentials = new NetworkCredential("Sandbox", "asdkljWuzux2", Environment.UserDomainName);

            var hubNames = arguments.Hubs.SelectMany(x => x.Split(',')).ToArray();
            var hubAgentManager = _hubAgentFactory.Create(hubNames, credentials);
            hubAgentManager.Connector.StateChanged += Connection_OnStateChanged;

            System.Console.WriteLine("Connecting to SignalR server...");
            Task task = null;
            Exception exception = null;
            try
            {
                var timeout = arguments.Timeout;
                task = hubAgentManager.Connector.Connect();
                task.Wait(timeout);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (exception != null || task == null || task.IsFaulted || task.Exception != null)
                {
                    System.Console.WriteLine("Error connecting to SignalR server...");
                    exception = task?.Exception?.InnerExceptions.FirstOrDefault()?.GetBaseException() ?? exception;
                    if (exception != null)
                        System.Console.WriteLine(exception.Message);
                }
                else
                {
                    if (hubAgentManager.Connector.Connected)
                    {
                        System.Console.WriteLine($"Connected! #{hubAgentManager.Connector.ConnectionId}");
                    }
                }
            }

            //_hubs = clientHubManager;
            _hubAgentManager = hubAgentManager;
        }



        private static void VerbReconnectHub(CommandArguments a)
        {
            if (_hubAgentManager == null)
            {
                System.Console.WriteLine("No server initialized...");
                return;
            }

            var p = new FluentCommandLineParser<ReconnectArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.AutoReconnect)
                .As('a', "auto")
                .SetDefault(false)    //
                .WithDescription("Whether to automatically reconnect");
            //p.Setup(x => x.ReconnectInterval)
            //    .As('t')
            //    .SetDefault(TimeSpan.FromSeconds(3))
            //    .WithDescription("The duration between reconnect tries");
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
            arguments = arguments ?? new ReconnectArguments();

            if (arguments.AutoReconnect)
            {
                System.Console.WriteLine("Re-connecting to hubs (auto)...");
                var r = _hubAgentManager.Connector.EnsureReconnecting();
                System.Console.WriteLine("EnsureReconnecting: " + r);
            }
            else
            {
                System.Console.WriteLine("Re-connecting to hubs (manual)...");

                Task task = null;
                Exception exception = null;
                try
                {
                    var timeout = arguments.Timeout;
                    task = _hubAgentManager.Connector.Connect();
                    task.Wait(timeout);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    if (exception != null || task == null || task.IsFaulted || task.Exception != null)
                    {
                        System.Console.WriteLine("Error re-connecting to SignalR server...");
                        exception = task?.Exception?.InnerExceptions.FirstOrDefault()?.GetBaseException() ?? exception;
                        if (exception != null)
                            System.Console.WriteLine(exception.Message);
                    }
                    else
                    {
                        if (_hubAgentManager.Connector.Connected)
                        {
                            System.Console.WriteLine($"Connected! #{_hubAgentManager.Connector.ConnectionId}");
                        }
                    }
                }
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
            if (_hubAgentManager == null)
            {
                System.Console.WriteLine("No server initialized...");
                return;
            }
            
            arguments = arguments ?? new DisconnectArguments();
            if (arguments.Force)
            {
                var msg = arguments.ForceMessage;
                Exception error = new OperationCanceledException(msg);
                _hubAgentManager.Connector.Disconnect();
                //_hubAgentManager.Connector.Disconnect(error);
            }
            else
            {
                _hubAgentManager.Connector.Disconnect();
            }

            _hubAgentManager.Connector.Dispose();
            _hubAgentManager.Dispose();
            _hubAgentManager = null;
        }



        private static void Connection_OnStateChanged(object sender, HubConnectionStateChange stateChange)
        {
            //System.Console.WriteLine("Connection changed: {0} => {1}", stateChange.OldState, stateChange.NewState);
        }



        private static void VerbSubscribeHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<SubscribeArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.HubName)
                .As('h', "hub")
                .WithDescription("Hub name");
            p.Setup(x => x.EventName)
                .As('e', "event")
                .WithDescription("Event name");
            p.SetupHelp("help");

            var args = a.Verb == "subscribe"
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
                SubscribeHub(p.Object);
            }
        }


        private static void SubscribeHub(SubscribeArguments arguments)
        {
            if (_hubAgentManager == null)
            {
                System.Console.WriteLine("No server initialized...");
                return;
            }
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            if (string.IsNullOrWhiteSpace(arguments.HubName))
                throw new ArgumentNullException(nameof(arguments.HubName));

            var hub = _hubAgentManager.GetHub(arguments.HubName);
            if (hub == null)
            {
                System.Console.WriteLine("Hub not initialized...");
                return;
            }
            var subscription = hub.Subscribe(arguments.EventName);
            if (subscription != null)
            {
                subscription.Received -= OnHubReceive;
                subscription.Received += OnHubReceive;
            }
        }




        private static void VerbUnsubscribeHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<UnsubscribeArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.HubName)
                .As('h', "hub")
                .WithDescription("Hub name");
            p.Setup(x => x.EventName)
                .As('e', "event")
                .WithDescription("Event name");
            p.SetupHelp("help");

            var args = a.Verb == "unsubscribe"
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
                UnsubscribeHub(p.Object);
            }
        }


        private static void UnsubscribeHub(UnsubscribeArguments arguments)
        {
            if (_hubAgentManager == null)
            {
                System.Console.WriteLine("No server initialized...");
                return;
            }
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            if (string.IsNullOrWhiteSpace(arguments.HubName))
                throw new ArgumentNullException(nameof(arguments.HubName));

            var hub = _hubAgentManager.GetHub(arguments.HubName);
            if (hub == null)
            {
                System.Console.WriteLine("Hub not initialized...");
                return;
            }

            var subscription = hub.Subscribe(arguments.EventName);
            if (subscription != null)
            {
                subscription.Received -= OnHubReceive;
            }
        }




        private static void VerbSendToHub(CommandArguments a)
        {
            var p = new FluentCommandLineParser<SendToHubArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.HubName)
                .As('h', "hub")
                .WithDescription("Hub name");
            p.Setup(x => x.Method)
                .As('m', "method")
                .WithDescription("Method name");
            p.Setup(x => x.Args)
                .As('a', "args")
                .WithDescription("Method arguments");
            p.Setup(x => x.Queuable)
                .As('q', "queuable")
                .WithDescription("Whether this message should be queued if not currently connected");
            p.SetupHelp("help");

            var args = a.Verb == "send" ||
                       a.Verb == "send-to-hub"
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
                SendToHub(p.Object);
            }
        }


        private static void SendToHub(SendToHubArguments arguments)
        {
            if (_hubAgentManager == null)
            {
                System.Console.WriteLine("No server initialized...");
                return;
            }

            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            if (string.IsNullOrWhiteSpace(arguments.HubName))
                throw new ArgumentNullException(nameof(arguments.HubName));
            if (string.IsNullOrWhiteSpace(arguments.Method))
                throw new ArgumentNullException(nameof(arguments.Method));
            
            var hub = _hubAgentManager.GetHub(arguments.HubName);
            if (hub == null)
            {
                System.Console.WriteLine("Hub not initialized...");
                return;
            }

            object[] args = arguments.Args?.Cast<object>().ToArray() ?? new object[0];
            var message = new HubMessage
            {
                //Hub = arguments.HubName,
                Method = arguments.Method,
                Args = args,
                Queuable = arguments.Queuable,
            };
            var timeout = arguments.Timeout;
            var task = hub.Invoke(message);
            task.Wait(timeout);
        }



        private static void OnHubReceive(IHubSubscription subscription, IList<JToken> list)
        {
            System.Console.WriteLine($"::{subscription.HubName}.{subscription.EventName}::");
            foreach (var tkn in list)
            {
                System.Console.WriteLine($"{subscription.EventName} token: {tkn}");
            }
        }





        private static void VerbZeroConfResolve(CommandArguments a)
        {
            var p = new FluentCommandLineParser<ZeroConfResolveArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.Host)
                .As('h', "host")
                .WithDescription("Host name");
            p.Setup(x => x.Type)
                .As('t', "type")
                .WithDescription("Type. (ex: '_http._tcp')");
            p.Setup(x => x.Domain)
                .As('d', "domain")
                .WithDescription("Domain. (ex: 'local')");
            p.SetupHelp("help");

            var args = a.Verb == "zeroconf resolve" ||
                       a.Verb == "zeroconf-resolve"
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
                ZeroConfResolve(p.Object);
            }
        }


        private static void ZeroConfResolve(ZeroConfResolveArguments arguments)
        {
            arguments = arguments ?? new ZeroConfResolveArguments();



            if (_zeroconfBrowser == null)
            {
                _zeroconfBrowser = new ZeroconfService.NetServiceBrowser();
                _zeroconfBrowser.DidFindDomain += (browser, name, coming) =>
                {
                    System.Console.WriteLine($"DidFindDomain: {name} [{coming}]");
                };
                _zeroconfBrowser.DidFindService += (browser, service, coming) =>
                {
                    System.Console.WriteLine($"DidFindService: {service} ({service.Name}) [{coming}]");
                };
                _zeroconfBrowser.DidRemoveDomain += (browser, name, coming) =>
                {
                    System.Console.WriteLine($"DidRemoveDomain: {name} [{coming}]");
                };
                _zeroconfBrowser.DidRemoveService += (browser, service, coming) =>
                {
                    System.Console.WriteLine($"DidRemoveService: {service} ({service.Name}) [{coming}]");
                };
            }
            System.Console.WriteLine("Stopping ZeroConf browser...");
            _zeroconfBrowser.Stop();



            //_zeroconfBrowser.SearchForBrowseableDomains();
            //_zeroconfBrowser.SearchForRegistrationDomains();
            System.Console.WriteLine($"Searching for ZeroConf services with type '{arguments.Type} and domain '{arguments.Domain}'");
            _zeroconfBrowser.SearchForService(arguments.Type, arguments.Domain);
        }





        private static void VerbZeroConfPublish(CommandArguments a)
        {
            var p = new FluentCommandLineParser<ZeroConfPublishArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.Name)
                .As('n', "name")
                .WithDescription("Name of the record");
            p.Setup(x => x.Type)
                .As('t', "type")
                .WithDescription("Type. (ex: '_http._tcp')");
            p.Setup(x => x.Domain)
                .As('d', "domain")
                .WithDescription("Domain. (ex: 'local')");
            p.Setup(x => x.Port)
                .As('p', "port")
                .WithDescription("Port");
            p.SetupHelp("help");

            var args = a.Verb == "zeroconf publish" ||
                       a.Verb == "zeroconf-publish"
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
                ZeroConfPublish(p.Object);
            }
        }


        private static void ZeroConfPublish(ZeroConfPublishArguments arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var net = new ZeroconfService.NetService(arguments.Domain, arguments.Type, arguments.Name, arguments.Port);
            net.DidPublishService += service =>
            {
                System.Console.WriteLine($"DidPublishService: {service} ({service.Name})");
            };
            net.DidNotPublishService += (service, exception) =>
            {
                System.Console.WriteLine($"DidNotPublishService: {service} ({service.Name}) [{exception.ErrorType}] :: {exception.Function} - {exception.Message}");
            };
            net.DidResolveService += service =>
            {
                System.Console.WriteLine($"DidResolveService: {service} ({service.Name})");
            };
            net.DidNotResolveService += (service, exception) =>
            {
                System.Console.WriteLine($"DidNotResolveService: {service} ({service.Name}) [{exception.ErrorType}] :: {exception.Function} - {exception.Message}");
            };
            net.DidUpdateTXT += service =>
            {
                System.Console.WriteLine($"DidUpdateTXT: {service} ({service.Name})");
            };
            System.Console.WriteLine("Publishing ZeroConf record...");
            net.Publish();
            net.StartMonitoring();
            System.Console.WriteLine($"Name: {net.Name}");
            System.Console.WriteLine($"HostName: {net.HostName}");
            System.Console.WriteLine($"Addresses: {string.Join(",", net.Addresses)}");
            System.Console.WriteLine($"Port: {net.Port}");
            System.Console.WriteLine($"Type: {net.Type}");
            System.Console.WriteLine($"Domain: {net.Domain}");

            while (true)
            {
                var input = System.Console.ReadLine();
                System.Console.WriteLine("Write 'stop' to end the ZeroConf publish");
                if (input == "stop")
                    break;
            }

            net.Stop();
            net.StopMonitoring();
            net.Dispose();
        }


    }
}
