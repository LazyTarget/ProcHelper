using System.ServiceProcess;

namespace FullCtrl.Base
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