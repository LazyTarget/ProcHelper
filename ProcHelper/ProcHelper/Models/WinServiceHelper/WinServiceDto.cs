using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace ProcHelper
{
    public class WinServiceDto
    {
        private readonly ServiceController _winServiceController;

        public WinServiceDto(ServiceController winServiceController)
        {
            _winServiceController = winServiceController;
        }

        public ServiceController GetBase()
        {
            return _winServiceController;
        }


        public string ServiceName
        {
            get { return _winServiceController.ServiceName; }
        }

        public string DisplayName
        {
            get { return _winServiceController.DisplayName; }
        }

        public string MachineName
        {
            get { return _winServiceController.MachineName; }
        }

        public ServiceControllerStatus Status
        {
            get { return _winServiceController.Status; }
        }

        public bool CanPauseAndContinue
        {
            get { return _winServiceController.CanPauseAndContinue; }
        }

        public bool CanStop
        {
            get { return _winServiceController.CanStop; }
        }

        public bool CanShutdown
        {
            get { return _winServiceController.CanShutdown; }
        }

        public ServiceType ServiceType
        {
            get { return _winServiceController.ServiceType; }
        }

        public List<WinServiceDto> ServicesDependedOn
        {
            get { return _winServiceController.ServicesDependedOn.Select(x => new WinServiceDto(x)).ToList(); }
            //get { return null; }
        }

        public List<WinServiceDto> DependentServices
        {
            //get { return _winServiceController.DependentServices.Select(x => new WinServiceDto(x)).ToList(); }
            get { return null; }        // todo: prevent infinite loops
        }
    }
}
