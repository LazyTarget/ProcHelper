using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remotus.Base
{
    [Obsolete]
    public interface IClientInfo
    {
        string ClientID { get; }
        int ApiVersion { get; }
        Uri ApiAddress { get; }
        IServerInfo ServerInfo { get; }

        Task<IEnumerable<IPlugin>> GetPlugins();
    }
}
