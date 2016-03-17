using System;

namespace Remotus.Base
{
    public interface IService : IDisposable
    {
        ServiceStatus Status { get; }
        void Start();
        void Stop();
    }
}
