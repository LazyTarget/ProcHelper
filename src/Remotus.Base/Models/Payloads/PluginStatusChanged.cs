namespace Remotus.Base.Payloads
{
    public class PluginStatusChanged
    {
        public PluginStatusChanged()
        {
            
        }

        public PluginStatusChanged(string agentId, IComponentDescriptor plugin, ServiceStatus oldStatus, ServiceStatus newStatus)
            : this()
        {
            AgentId = agentId;
            Plugin = plugin;
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }

        public string AgentId { get; set; }

        public IComponentDescriptor Plugin { get; set; }

        public ServiceStatus OldStatus { get; set; }

        public ServiceStatus NewStatus { get; set; }
    }
}
