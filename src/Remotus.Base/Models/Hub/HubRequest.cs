using System;

namespace Remotus.Base.Models.Hub
{
    public class HubRequest
    {
        public string AgentId { get; set; }                 // nessesary?
        public string ConnectionId { get; set; }            // nessesary?
        public string HubName { get; set; }
        public IHubMessage Message { get; set; }
        public DateTime? TimeQueued { get; set; }
        public DateTime? TimeSent { get; set; }
    }
}
