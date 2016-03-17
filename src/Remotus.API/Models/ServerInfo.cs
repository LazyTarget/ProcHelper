using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.API
{
    public class ServerInfo : IServerInfo
    {
        public ServerInfo()
        {
            
        }


        public string InstanceID { get; set; }
        public int ApiVersion { get; set; }
        public Uri ApiAddress { get; set; }

        public Task<IEnumerable<IClientInfo>> GetClients()
        {
            throw new NotImplementedException();
        }
    }
}
