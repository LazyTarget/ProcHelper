using System;

namespace Remotus.Base
{
    public interface IService : IDisposable
    {
        ServiceStatus Status { get; }
        void Init(IExecutionContext context);
        void Start();
        void Stop();
    }
}
