using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public interface IService : IDisposable
    {
        ServiceStatus Status { get; }
        event EventHandler<ServiceStatusChangedEventArgs> OnStatusChanged;

        Task Start();
        Task Stop();
    }
}
