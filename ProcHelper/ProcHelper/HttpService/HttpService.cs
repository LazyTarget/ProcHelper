using System.Collections.Generic;
using System.Linq;

namespace ProcHelper
{
    public class HttpService : ServiceStack.Service
    {
        private IProcessHelper _processHelper = new ProcessHelper();
        private IWinServiceHelper _winServiceHelper = new WinServiceHelper();


        #region Services

        public ProcessesResponse Any(GetProcessesRequest request)
        {
            List<ProcessDto> processes;
            if (request != null && !string.IsNullOrEmpty(request.Name))
                processes = _processHelper.GetProcesses(request.Name);
            else
                processes = _processHelper.GetProcesses();

            var response = new ProcessesResponse
            {
                Processes = processes,
            };
            return response;
        }


        public StartProcessResponse Any(StartProcessRequest request)
        {
            var process = _processHelper.StartProcess(request.FileName, request.Arguments, request.WorkingDirectory);
            var response = new StartProcessResponse
            {
                Process = process,
            };
            return response;
        }


        public KillProcessResponse Any(KillProcessRequest request)
        {
            var process = _processHelper.KillProcess(request.ProcessID);
            var response = new KillProcessResponse
            {
                Process = process,
            };
            return response;
        }

        #endregion


        #region WinServices


        public WinServicesResponse Any(GetWinServicesRequest request)
        {
            var services = _winServiceHelper.GetServices();
            if (request != null && !string.IsNullOrEmpty(request.Name))
                services = services.Where(x => x.ServiceName == request.Name).ToList();
            var response = new WinServicesResponse
            {
                Services = services,
            };
            return response;
        }

        public StartWinServiceResponse Any(StartWinServiceRequest request)
        {
            var argArray = request.Arguments != null ? request.Arguments.Split(' ') : null;
            var service = _winServiceHelper.StartService(request.ServiceName, argArray);
            var response = new StartWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public PauseWinServiceResponse Any(PauseWinServiceRequest request)
        {
            var service = _winServiceHelper.PauseService(request.ServiceName);
            var response = new PauseWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public ContinueWinServiceResponse Any(ContinueWinServiceRequest request)
        {
            var service = _winServiceHelper.ContinueService(request.ServiceName);
            var response = new ContinueWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public StopWinServiceResponse Any(StopWinServiceRequest request)
        {
            var service = _winServiceHelper.StopService(request.ServiceName);
            var response = new StopWinServiceResponse
            {
                Service = service,
            };
            return response;
        }
        


        #endregion

    }
}
