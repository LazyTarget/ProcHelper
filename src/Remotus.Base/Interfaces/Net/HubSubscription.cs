using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Remotus.Base
{
    public class HubSubscription : IHubSubscription
    {
        public string HubName { get; set; }
        public string EventName { get; set; }
        public event Action<IList<JToken>> Received;

        public void Invoke(IList<JToken> obj)
        {
            Received?.Invoke(obj);
        }

        public void Dispose()
        {
            Received = null;
        }
    }
}