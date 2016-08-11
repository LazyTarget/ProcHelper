using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Remotus.Base.Interfaces.Net
{
    public class HubSubscriptionEvent : EventArgs
    {
        public HubSubscriptionEvent(IHubSubscription subscription, IList<JToken> data, Exception error)
        {
            Subscription = subscription;
            Data = data;
            Error = error;
        }

        public IHubSubscription Subscription { get; }
        public IList<JToken> Data { get; } 
        public Exception Error { get; }
    }
}