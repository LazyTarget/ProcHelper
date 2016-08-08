using System;

namespace Remotus.API.Models
{
    public class HubHandshake
    {
        public string AgentId { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string UserDomainName { get; set; }
        public Uri Address { get; set; }
    }
}
