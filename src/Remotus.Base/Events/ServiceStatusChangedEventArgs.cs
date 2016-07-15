using System;

namespace Remotus.Base
{
    public class ServiceStatusChangedEventArgs : EventArgs
    {
        public ServiceStatusChangedEventArgs(ServiceStatus oldStatus, ServiceStatus newStatus)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }

        public ServiceStatus OldStatus { get; }
        public ServiceStatus NewStatus { get; }
    }
}
