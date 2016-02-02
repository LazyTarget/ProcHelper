using System.ServiceProcess;

namespace FullCtrl.Base
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