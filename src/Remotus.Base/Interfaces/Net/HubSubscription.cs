using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Remotus.Base.Observables;

namespace Remotus.Base.Interfaces.Net
{
    public class HubSubscription : IHubSubscription, IObservable<HubSubscriptionEvent>
    {
        private readonly object _observerLock = new object();
        private readonly IObservable<IList<JToken>> _parentObservable;
        private readonly BroadcasterBase<HubSubscriptionEvent> _observable;

        public HubSubscription(IObservable<IList<JToken>> observable)
        {
            _parentObservable = observable;
            _observable = new AsyncBroadcaster<HubSubscriptionEvent>();
        }

        public string HubName { get; set; }
        public string EventName { get; set; }
        

        public IDisposable Subscribe(IObserver<HubSubscriptionEvent> observer)
        {
            var delegator = new DelegateObserver<IList<JToken>>(
                onNext: (list) =>
                {
                    var args = new HubSubscriptionEvent(this, data: list, error: null);
                    _observable?.OnNext(args);
                },
                onError: (error) =>
                {
                    _observable.OnError(error);
                },
                onCompleted: () =>
                {
                    _observable.OnCompleted();
                }
            );
            var parentSub = _parentObservable.Subscribe(delegator);
            var sub = _observable.Subscribe(observer);

            return new DelegateDisposable(() =>
            {
                lock (_observerLock)
                {
                    parentSub.Dispose();
                    sub.Dispose();
                }
            });
        }

    }
}