using System;

namespace Remotus.Base.Interfaces.Net
{
    public interface IHubSubscription : IObservable<HubSubscriptionEvent>
    {
        string HubName { get; }
        string EventName { get; }
    }
}
