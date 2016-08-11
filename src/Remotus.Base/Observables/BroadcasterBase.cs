﻿using System;

namespace Remotus.Base.Observables
{
    public abstract class BroadcasterBase<T> : IBroadcaster<T>, IDisposable
    {
        public abstract bool HasObservers { get; }

        public abstract bool IsDisposed { get; }

        public abstract bool IsComplete { get; }

        public abstract void OnNext(T value);

        public abstract void OnError(Exception error);

        public abstract void OnCompleted();

        public abstract IDisposable Subscribe(IObserver<T> observer);

        public abstract void Dispose();
    }
}
