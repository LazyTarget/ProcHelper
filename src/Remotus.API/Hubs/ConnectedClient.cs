using System.Collections.Generic;
using Remotus.API.Models;

namespace Remotus.API.Hubs
{
    public class ConnectedClient
    {
        public ConnectedClient()
        {
            Hubs = new HashSet<string>();
        }

        public string ConnectionId { get; set; }
        public bool Connected { get; set; }
        public HubHandshake Handshake { get; set; }
        public HashSet<string> Hubs { get; private set; } 
    }
}
