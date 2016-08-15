using System;
using Remotus.API.Data;

namespace Remotus.API.Net
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
            RecipeRepository = new RecipeRepository();
        }
        
        public ClientManager ClientManager { get; private set; }
        public ConnectionManager ConnectionManager { get; private set; }
        public RecipeRepository RecipeRepository { get; private set; }

    }
}
