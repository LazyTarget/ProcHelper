using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Remotus.Base
{
    public interface IHubSubscription : IDisposable
    {
        string HubName { get; }
        string EventName { get; }
        event Action<IHubSubscription, IList<JToken>> Received;
    }
}
