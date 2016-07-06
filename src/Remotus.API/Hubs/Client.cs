using Remotus.API.Models;

namespace Remotus.API.Hubs
{
    public class Client
    {
        public string ConnectionId { get; set; }
        public bool Connected { get; set; }
        public HubHandshake Handshake { get; set; }
    }
}
