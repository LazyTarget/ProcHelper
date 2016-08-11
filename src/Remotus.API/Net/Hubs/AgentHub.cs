using System.Reflection;
using Remotus.Base.Payloads;

namespace Remotus.API.Net.Hubs
{
    public class AgentHub : HubBase
    {
        // Events:
        // OnPluginStatusChanged(PluginStatusChanged model)

        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;


        public void GetPlugins()
        {
            var plugins = Program.Service._plugins.Values;
            foreach (var agentPlugin in plugins)
            {
                var msg = $"Plugin: {agentPlugin.Name}/{agentPlugin.Version} [{agentPlugin.Status}]";
                Clients.All.OnMessage(msg);
            }
        }


        public void StartPlugin(string pluginName)
        {
            if (!string.IsNullOrWhiteSpace(pluginName))
            {
                var plugin = Program.Service._plugins.ContainsKey(pluginName)
                    ? Program.Service._plugins[pluginName]
                    : null;
                if (plugin != null)
                {
                    Program.Service.StartPlugin(plugin);
                }
            }
        }


        public void StopPlugin(string connectionId, string pluginName)
        {
            if (!string.IsNullOrWhiteSpace(pluginName))
            {
                var plugin = Program.Service._plugins.ContainsKey(pluginName)
                    ? Program.Service._plugins[pluginName]
                    : null;
                if (plugin != null)
                {
                    Program.Service.StopPlugin();
                }

                Clients.Client(connectionId).StopPlugin(pluginName);
            }
        }


        public void OnPluginStatusChanged(PluginStatusChanged model)
        {
            if (!string.IsNullOrWhiteSpace(model?.Plugin?.Name))
            {
                var plugin = Program.Service._plugins.ContainsKey(model.Plugin.Name)
                    ? Program.Service._plugins[model.Plugin.Name]
                    : null;
                if (plugin != null)
                {
                    plugin.Status = model.NewStatus;
                }
            }

            Clients.All.OnPluginStatusChanged(model);
        }

    }
}
