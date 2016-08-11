using System;
using System.Threading;

namespace Remotus.Base.Observables
{
    public sealed class DelegateObserver<T> : IObserver<T>, IDisposable
    {
        private readonly Action _onCompleted;
        private readonly Action<Exception> _onError;
        private readonly Action<T> _onNext;
        private int _isStopped;

        public DelegateObserver(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            if (_isStopped == 0)
                _onNext?.Invoke(value);
        }

        public void OnError(Exception error)
        {
            if (Interlocked.Exchange(ref _isStopped, 1) == 0)
                _onError?.Invoke(error);
        }

        public void OnCompleted()
        {
            if (Interlocked.Exchange(ref _isStopped, 1) == 0)
                _onCompleted?.Invoke();
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _isStopped, 1);
        }
    }
}
