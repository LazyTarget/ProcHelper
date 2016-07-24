namespace Remotus.Base.Models.Payloads
{
    public class PluginStatusChanged
    {
        public PluginStatusChanged(string agentId, IComponentDescriptor plugin, ServiceStatus oldStatus, ServiceStatus newStatus)
        {
            AgentId = agentId;
            Plugin = plugin;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }

        public string AgentId { get; private set; }

        public IComponentDescriptor Plugin { get; private set; }

        public ServiceStatus OldStatus { get; private set; }

        public ServiceStatus NewStatus { get; private set; }
    }
}
