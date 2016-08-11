﻿using System;

namespace Remotus.Base.Observables
{
    public sealed class DelegateDisposable : IDisposable
    {
        private readonly Action _disposeAction;

        public DelegateDisposable(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction();
        }

        public static DelegateDisposable Create(Action disposeAction)
        {
            return new DelegateDisposable(disposeAction);
        }
    }
}
