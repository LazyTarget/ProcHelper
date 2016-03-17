using System.ServiceProcess;

namespace Remotus.Base
{
    public class StopWinServiceResponse
    {
        public ServiceControllerStatus Status
        {
            get { return Service.Status; }
        }

        public WinServiceDto Service { get; set; }

    }
}