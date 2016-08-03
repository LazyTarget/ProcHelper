using System;
using System.Collections.Generic;

namespace Remotus.Base
{
    public interface IHubAgentManager : IDisposable
    {
        IReadOnlyDictionary<string, IHubAgent> GetHubs();
        IHubAgent GetHub(string hubName);
        IHubConnector Connector { get; }
    }
}
