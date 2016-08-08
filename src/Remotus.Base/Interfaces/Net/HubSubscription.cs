using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Remotus.Base.Interfaces.Net
{
    public class HubSubscription : IHubSubscription
    {
        public string HubName { get; set; }
        public string EventName { get; set; }
        public event Action<IHubSubscription, IList<JToken>> Received;

        public void Invoke(IList<JToken> obj)
        {
            Received?.Invoke(this, obj);
        }

        public void Dispose()
        {
            Received = null;
        }
    }
}