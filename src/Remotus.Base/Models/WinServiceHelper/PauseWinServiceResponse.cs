using System.ServiceProcess;

namespace Remotus.Base
{
    public class PauseWinServiceResponse
    {
        public ServiceControllerStatus Status
        {
            get { return Service.Status; }
        }

        public WinServiceDto Service { get; set; }

    }
}