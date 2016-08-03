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
using Remotus.API.Hubs.Client;
using Remotus.API.v1;
using Remotus.Base;
using Remotus.Base.Models.Hub;
using Remotus.Base.Models.Payloads;
using ExecutionContext = Remotus.API.ExecutionContext;

namespace Remotus.Launcher
{
    public class PluginManager : IDisposable
    {
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
            string[] hubNames = new [] { "AgentHub", "ServerHub", "DiagnosticsHub" };
            ICredentials credentials = CredentialCache.DefaultCredentials;
            IDictionary<string, string> queryString = null;
            
            _hubAgentManager = _hubAgentFactory.Create(hubNames, credentials, queryString);

            var serverHub = _hubAgentManager.GetHub("ServerHub");
            serverHub.Subscribe("StartPlugin").Received += HubEvent_OnStartPlugin;
            serverHub.Subscribe("StopPlugin").Received += HubEvent_OnStopPlugin;

            var agentHub = _hubAgentManager.GetHub("AgentHub");
            var diagHub = _hubAgentManager.GetHub("DiagnosticsHub");

            try
            {
                await _hubAgentManager.Connect();
            }
            catch (Exception ex)
            {
#if DEBUG
                var r = _hubAgentManager.EnsureReconnecting();
#else
                throw;
#endif
            }
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
        }


        private async Task Start()
        {
            if (_plugins == null || _plugins.Count <= 0)
                return;

            IClientInfo clientInfo = null;
            IExecutionContext context = new ExecutionContext
            {
                ClientInfo = clientInfo,
                Logger = new TraceLogger(),
                Remotus = new Remotus.API.v1.FullCtrlAPI(),
                //SignalR =  // todo: !
                HubAgentFactory = _hubAgentFactory,
            };

            var servicePlugins = _plugins.Values.Select(x => x.Instance).OfType<IServicePlugin>().ToList();
            foreach (var servicePlugin in servicePlugins)
            {
                // todo: Make plugin initialization async (load multiple plugins at the same time)

                try
                {
                    if (servicePlugin.Status != ServiceStatus.Initializing)
                    {
                        await servicePlugin.Init(context);
                    }

                    if (servicePlugin.Status != ServiceStatus.Running)
                    {
                        await servicePlugin.Start();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if (servicePlugins.Any())
            {
                _autoResetEvent.WaitOne();
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
                        await servicePlugin.Stop();
                    }
                }
                catch (Exception ex)
                {

                }
            }


            _autoResetEvent.Set();
        }

        private void HubEvent_OnStartPlugin(IList<JToken> obj)
        {
            var task = Start();
            task.Wait();
        }

        private void HubEvent_OnStopPlugin(IList<JToken> obj)
        {
            var task = Stop();
            task.Wait();
        }

        private void ServicePlugin_OnStatusChanged(object sender, ServiceStatusChangedEventArgs args)
        {
            if (_hubAgentManager == null)
                return;

            //var agentHub = GlobalHost.ConnectionManager.GetHubContext<AgentHub>();

            var plugin = sender as IServicePlugin;
            //var agentId = Program.Service?.ClientInfo?.ClientID;
            string agentId = null;
            var componentDesc = new ComponentDescriptor(plugin);
            var model = new PluginStatusChanged(agentId, componentDesc, args.OldStatus, args.NewStatus);


            var msg = string.Format("Plugin '{0}' status changed: {1} => {2}", model.Plugin?.Name, model.OldStatus, model.NewStatus);
            System.Console.WriteLine(msg);
            //agentHub.Clients.All.OnPluginStatusChanged(model);

            var message = new HubMessage
            {
                Method = "OnPluginStatusChanged",
                Args = new[] { model },
                Queuable = true,
            };
            var agentHub = _hubAgentManager.GetHub("AgentHub");
            var task = agentHub.Invoke(message);
            task.Wait();


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

            _hubAgentManager?.Disconnect();
            _hubAgentManager?.Dispose();
            _hubAgentManager = null;
        }
    }
}
