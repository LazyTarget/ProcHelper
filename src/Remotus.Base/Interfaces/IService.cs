using System;

namespace FullCtrl.Base
{
    public interface IService : IDisposable
    {
        ServiceStatus Status { get; }
        void Start();
        void Stop();
    }
}
