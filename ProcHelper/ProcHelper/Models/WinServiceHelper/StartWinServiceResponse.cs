using System.ServiceProcess;

namespace ProcHelper
{
    public class StartWinServiceResponse
    {
        public ServiceControllerStatus Status
        {
            get { return Service.Status; }
        }

        public WinServiceDto Service { get; set; }

    }
}