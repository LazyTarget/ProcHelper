using System;

namespace FullCtrl.Base
{
    public interface IServicePlugin : IPlugin, IDisposable
    {
        ServiceStatus Status { get; }
        void Start();
        void Stop();
    }

    public enum ServiceStatus
    {
        None,
        Starting,
        Running,
        Stopping,
        Stopped,
    }
}
