using System;
using System.Threading.Tasks;

namespace Remotus.Base.Interfaces.Net
{
    public interface IHubAgent : IDisposable
    {
        string HubName { get; }
        Task Invoke(IHubMessage message);
        IHubSubscription Observe(string eventName);
        IHubConnector Connector { get; }
    }
}
