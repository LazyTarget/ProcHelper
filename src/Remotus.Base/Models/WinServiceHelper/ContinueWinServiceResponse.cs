using System.ServiceProcess;

namespace Remotus.Base
{
    public class ContinueWinServiceResponse
    {
        public ServiceControllerStatus Status
        {
            get { return Service.Status; }
        }

        public WinServiceDto Service { get; set; }

    }
}