using System.ServiceProcess;

namespace ProcHelper
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