using System.Collections.Generic;
using System.Linq;

namespace ProcHelper
{
    public class Worker
    {
        private IProcessHelper _processHelper;
        private IWinServiceHelper _winServiceHelper;
        private IPowershellHelper _powershellHelper;

        public Worker()
        {
            _processHelper = new ProcessHelper();
            _winServiceHelper = new WinServiceHelper();
            _powershellHelper = new PowershellHelper(_processHelper);
        }



        #region ProcHelper

        public ProcessesResponse GetProcesses(GetProcessesRequest request)
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


        public StartProcessResponse StartProcess(StartProcessRequest request)
        {
            var process = _processHelper.StartProcess(request.FileName, request.Arguments, request.WorkingDirectory, request.RedirectStandardOutput);

            var response = new StartProcessResponse
            {
                Process = process,
            };
            if (request.RedirectStandardOutput)
            {
                var p = process.GetBase();
                response.StandardOutput = p.StandardOutput.ReadToEnd();
                response.StandardError = p.StandardError.ReadToEnd();
            }
            return response;
        }

        public KillProcessResponse KillProcess(KillProcessRequest request)
        {
            var process = _processHelper.KillProcess(request.ProcessID);
            var response = new KillProcessResponse
            {
                Process = process,
            };
            return response;
        }

        #endregion


        #region Powershell

        
        public PowershellResponse RunPowershellFile(PowershellFileRequest request)
        {
            var response = _powershellHelper.RunFile(request);
            return response;
        }

        public PowershellResponse RunPowershellQuery(PowershellQueryRequest request)
        {
            var response = _powershellHelper.RunQuery(request);
            return response;
        }

        #endregion


        #region WinServiceHelper


        public WinServicesResponse GetWinServices(GetWinServicesRequest request)
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

        public StartWinServiceResponse StartWinService(StartWinServiceRequest request)
        {
            var argArray = request.Arguments != null ? request.Arguments.Split(' ') : null;
            var service = _winServiceHelper.StartService(request.ServiceName, argArray);
            var response = new StartWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public PauseWinServiceResponse PauseWinService(PauseWinServiceRequest request)
        {
            var service = _winServiceHelper.PauseService(request.ServiceName);
            var response = new PauseWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public ContinueWinServiceResponse ContinueWinService(ContinueWinServiceRequest request)
        {
            var service = _winServiceHelper.ContinueService(request.ServiceName);
            var response = new ContinueWinServiceResponse
            {
                Service = service,
            };
            return response;
        }

        public StopWinServiceResponse StopWinService(StopWinServiceRequest request)
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
