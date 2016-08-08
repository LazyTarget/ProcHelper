using System;
using System.Collections.Generic;
using Remotus.API.Models;

namespace Remotus.API.Net.Models
{
    public class ConnectedClient
    {
        public ConnectedClient()
        {
            Hubs = new HashSet<string>();
        }

        public string ConnectionId { get; set; }
        public bool Connected { get; set; }
        public DateTime? TimeDisconnected { get; set; }
        public HubHandshake Handshake { get; set; }
        public HashSet<string> Hubs { get; private set; } 
    }
}
