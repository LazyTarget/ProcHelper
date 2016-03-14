using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullCtrl.Base
{
    public interface IServerInfo
    {
        string InstanceID { get; }
        int ApiVersion { get; }
        Uri ApiAddress { get; }

        Task<IEnumerable<IClientInfo>> GetClients();
    }
}
