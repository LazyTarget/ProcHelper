using System.ServiceProcess;

namespace FullCtrl.Base
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