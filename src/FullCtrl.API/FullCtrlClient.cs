using System;

namespace FullCtrl.API
{
    public class FullCtrlClient : IDisposable
    {
        private bool _started;
        private bool _disposed;

        public void Start()
        {
            if (_started)
                return;
            _started = true;
        }

        public void Stop()
        {
            _started = false;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            Stop();
            _disposed = true;
        }
    }
}