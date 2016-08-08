using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lux.IO;
using Newtonsoft.Json.Linq;
using Remotus.API;
using Remotus.API.Net.Client;
using Remotus.API.v1;
using Remotus.Base;
using Remotus.Base.Interfaces.Net;
using Remotus.Base.Net;
using Remotus.Base.Payloads;
using ExecutionContext = Remotus.API.ExecutionContext;

namespace Remotus.Launcher
{
    public class PluginManager : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);

        private IHubAgentManager _hubAgentManager;
        private readonly IHubAgentFactory _hubAgentFactory;
        private readonly IFileSystem _fileSystem;
        private readonly IDictionary<string, AgentPlugin> _plugins = new Dictionary<string, AgentPlugin>();
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);


        public PluginManager(IFileSystem fileSystem)
        {
            _hubAgentFactory = new LauncherHubAgentFactory();
            _fileSystem = fileSystem;
        }


        public async Task Run(string filePath)
        {
            await SetupHub();
            await Load(filePath);
            await Start();
        }


        private async Task SetupHub()
        {
            string[] hubNames = new [] { "AgentHub", "ServerHub", "TimeHub" };
            ICredentials credentials = CredentialCache.DefaultCredentials;
            IDictionary<string, string> queryString = null;
            
            _hubAgentManager = _hubAgentFactory.Create(hubNames, credentials, queryString);

            var agentHub = _hubAgentManager.GetHub("AgentHub");
            agentHub.Subscribe("StartPlugin").Received  += HubEvent_OnStartPlugin;
            agentHub.Subscribe("StopPlugin").Received   += HubEvent_OnStopPlugin;

            var serverHub = _hubAgentManager.GetHub("ServerHub");
            serverHub.Subscribe("StartPlugin").Received += HubEvent_OnStartPlugin;
            serverHub.Subscribe("StopPlugin").Received  += HubEvent_OnStopPlugin;

            var timeHub = _hubAgentManager.GetHub("TimeHub");
            timeHub.Subscribe("OnTick").Received        += HubEvent_OnTick;


            _hubAgentManager.Connector.ConnectContinuous();

            //Task task = null;
            //try
            //{
            //    task = _hubAgentManager.Connector.Connect();
            //    await task;
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    var r = _hubAgentManager.Connector.EnsureReconnecting();
            //}
        }


        private async Task Load(string filePath)
        {
            if (!_fileSystem.FileExists(filePath))
                throw new FileNotFoundException("Could not find plugin file", filePath);
            
            var instantiator = new Lux.TypeInstantiator();

            var ext = Path.GetExtension(filePath);
            if (ext == ".dll" || ext == ".plugin")
            {
                // Assembly
                try
                {
                    var assembly = Assembly.LoadFile(filePath);
                    var pluginTypes = assembly.ExportedTypes.Where(x => typeof (IPlugin).IsAssignableFrom(x)).ToList();

                    foreach (var pluginType in pluginTypes)
                    {
                        var obj = instantiator.Instantiate(pluginType);
                        var instance = (IPlugin)obj;
                        if (instance == null)
                            continue;
                        if (string.IsNullOrWhiteSpace(instance.Name))
                            continue;
                        
                        var servicePlugin = instance as IServicePlugin;
                        if (servicePlugin != null)
                        {
                            servicePlugin.OnStatusChanged -= ServicePlugin_OnStatusChanged;
                            servicePlugin.OnStatusChanged += ServicePlugin_OnStatusChanged;
                        }
                        else
                        {
                            //continue;
                        }

                        var plugin = new AgentPlugin
                        {
                            ID = instance.ID,
                            Name = instance.Name,
                            Version = instance.Version,

                            Loaded = true,
                            Instance = instance,
                            PluginInstanceType = pluginType,
                            PluginFile = filePath,
                        };
                        _plugins[plugin.Name] = plugin;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                // Manifest / Xml

                // TODO: instantiate using manifest info
                throw new NotImplementedException();
            }


            _log.Debug(() => $"Plugins loaded: " + _plugins.Count);
            _log.Debug(() => $"Service plugins found: " + _plugins.Count(x => x.Value.Instance is IServicePlugin));
        }


        private async Task Start()
        {
            if (_plugins == null || _plugins.Count <= 0)
                return;
            
            var servicePlugins = _plugins.Values.Select(x => x.Instance).OfType<IServicePlugin>().ToList();
            if (servicePlugins.Any())
            {
                IClientInfo clientInfo = null;
                IExecutionContext context = new ExecutionContext
                {
                    ClientInfo = clientInfo,
                    Logger = new TraceLogger(),
                    Remotus = new Remotus.API.v1.FullCtrlAPI(),
                    //SignalR =  // todo: !
                    HubAgentFactory = _hubAgentFactory,
                };

                foreach (var servicePlugin in servicePlugins)
                {
                    // todo: Make plugin initialization async (load multiple plugins at the same time)

                    try
                    {
                        if (servicePlugin.Status != ServiceStatus.Initializing)
                        {
                            _log.Info(() => $"Service plugin '{servicePlugin.Name}', initializing...");
                            await servicePlugin.Init(context);
                        }

                        if (servicePlugin.Status != ServiceStatus.Running)
                        {
                            _log.Info(() => $"Service plugin '{servicePlugin.Name}', starting...");
                            await servicePlugin.Start();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                _autoResetEvent.WaitOne();
            }
            else
            {
                _log.Info(() => $"No service plugins found");
            }
        }

        
        private async Task Stop()
        {
            if (_plugins == null || _plugins.Count <= 0)
                return;
            
            var servicePlugins = _plugins.Values.Select(x => x.Instance).OfType<IServicePlugin>().ToList();
            foreach (var servicePlugin in servicePlugins)
            {
                try
                {
                    if (servicePlugin.Status != ServiceStatus.Stopping &&
                        servicePlugin.Status != ServiceStatus.Stopped)
                    {
                        _log.Info(() => $"Service plugin '{servicePlugin.Name}', stopping...");
                        await servicePlugin.Stop();
                    }
                }
                catch (Exception ex)
                {

                }
            }


            _autoResetEvent.Set();
        }

        private void HubEvent_OnStartPlugin(IHubSubscription subscription, IList<JToken> list)
        {
            var task = Start();
            task.Wait();
        }

        private void HubEvent_OnStopPlugin(IHubSubscription subscription, IList<JToken> list)
        {
            var task = Stop();
            task.Wait();
        }

        private void HubEvent_OnTick(IHubSubscription subscription, IList<JToken> list)
        {
            System.Console.WriteLine("::TimeHub.OnTick()::");
            foreach (var tkn in list)
            {
                System.Console.WriteLine("OnTick tkn: " + tkn);
            }
        }


        private void ServicePlugin_OnStatusChanged(object sender, ServiceStatusChangedEventArgs args)
        {
            if (_hubAgentManager == null)
                return;

            //var agentHub = GlobalHost.ConnectionManager.GetHubContext<AgentHub>();

            var plugin = sender as IServicePlugin;
            //var agentId = Program.Service?.ClientInfo?.ClientID;
            string agentId = null;
            var componentDesc = ComponentDescriptor.Create(plugin);
            var model = new PluginStatusChanged(agentId, componentDesc, args.OldStatus, args.NewStatus);


            var msg = string.Format("Plugin '{0}' status changed: {1} => {2}", model.Plugin?.Name, model.OldStatus, model.NewStatus);
            System.Console.WriteLine(msg);
            _log.Info(msg);
            //agentHub.Clients.All.OnPluginStatusChanged(model);

            Task task = null;
            try
            {
                var message = new HubMessage
                {
                    Method = "OnPluginStatusChanged",
                    Args = new[] { model },
                    Queuable = true,
                };
                var timeout = TimeSpan.FromSeconds(30);
                var agentHub = _hubAgentManager.GetHub("AgentHub");
                task = agentHub.Invoke(message);
                task.Wait(timeout);
            }
            catch (Exception ex)
            {

            }


            //var msgModel = new DebugMessage(agentId, null, msg, 2);
            //var diagHub = GlobalHost.ConnectionManager.GetHubContext<DiagnosticsHub>();
            //diagHub.Clients.Group("ListenTo_" + agentId).OnDebugMessage(msgModel);
        }

        public void Dispose()
        {
            var task = Stop();
            task.Wait(TimeSpan.FromSeconds(3));

            foreach (var agentPlugin in _plugins)
            {
                agentPlugin.Value?.Instance?.Dispose();
            }

            if (_hubAgentManager != null)
            {
                _hubAgentManager?.Connector.Disconnect();
                _hubAgentManager?.Connector.Dispose();
                _hubAgentManager?.Dispose();
                _hubAgentManager = null;
            }
        }
    }
}
