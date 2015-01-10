using System.Collections.Generic;

namespace ProcHelper
{
    public class HttpService : ServiceStack.Service
    {
        private IProcessHelper _helper = new ProcessHelper();


        public ProcessesResponse Any(GetProcessesRequest request)
        {
            List<ProcessDto> processes;
            if (request != null && !string.IsNullOrEmpty(request.Name))
                processes = _helper.GetProcesses(request.Name);
            else
                processes = _helper.GetProcesses();

            var response = new ProcessesResponse
            {
                Processes = processes,
            };
            return response;
        }


        public StartProcessResponse Any(StartProcessRequest request)
        {
            var process = _helper.StartProcess(request.FileName, request.Arguments, request.WorkingDirectory);
            var response = new StartProcessResponse
            {
                Process = process,
            };
            return response;
        }


        public KillProcessResponse Any(KillProcessRequest request)
        {
            var process = _helper.KillProcess(request.ProcessID);
            var response = new KillProcessResponse
            {
                Process = process,
            };
            return response;
        }

    }
}
