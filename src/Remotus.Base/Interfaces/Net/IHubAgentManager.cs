using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface IHubAgentManager : IDisposable
    {
        IReadOnlyDictionary<string, IHubAgent> GetHubs();
        IHubAgent GetHub(string hubName);

        Task Connect();
        void Disconnect();
    }
}
