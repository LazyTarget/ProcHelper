using System.ServiceProcess;

namespace FullCtrl.Base
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