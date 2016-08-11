using System;
using Remotus.Base;

namespace Remotus.API
{
    //public class AgentPlugin
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string Version { get; set; }
    //    public ServiceStatus Status { get; set; }
    //}

    //public class LoadedPlugin : AgentPlugin
    //{
    //    public bool Loaded { get; set; }
    //    public Type PluginInstanceType { get; set; }
    //    public IPlugin Instance { get; set; }
    //    public string PluginFile { get; set; }
    //}

    public class AgentPlugin
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public ServiceStatus Status { get; set; }

        public bool Loaded { get; set; }
        public Type PluginInstanceType { get; set; }
        public IPlugin Instance { get; set; }
        public string PluginFile { get; set; }
    }

    public class LoadedPlugin : AgentPlugin
    {
    }
}
