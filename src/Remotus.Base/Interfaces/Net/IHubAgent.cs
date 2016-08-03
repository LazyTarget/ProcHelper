using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface IHubAgent : IDisposable
    {
        string HubName { get; }
        Task Invoke(IHubMessage message);
        IHubSubscription Subscribe(string eventName);
        IHubConnector Connector { get; }
    }
}
