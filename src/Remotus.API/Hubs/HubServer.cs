using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remotus.API.Hubs
{
    public class HubServer
    {
        private static readonly Lazy<HubServer> _instance = new Lazy<HubServer>(() => new HubServer());
        public static HubServer Instance { get { return _instance.Value; } }



        private readonly object _locker = new object();

        private HubServer()
        {
            ClientManager = new ClientManager();
            ConnectionManager = new ConnectionManager(ClientManager, _locker);
        }
        
        public ClientManager ClientManager { get; private set; }
        public ConnectionManager ConnectionManager { get; private set; }

    }
}
