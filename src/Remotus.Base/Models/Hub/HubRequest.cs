using System;

namespace Remotus.Base.Models.Hub
{
    public class HubRequest
    {
        public string AgentId { get; set; }                 // nessesary?
        public string ConnectionId { get; set; }            // nessesary?
        public string HubName { get; set; }
        public IHubMessage Message { get; set; }
        public Action<object> OnProgress { get; set; }      // nessesary?
    }
}
