using System.Collections.Generic;

namespace ProcHelper
{
    public class HttpService : ServiceStack.Service
    {
        private IProcessHelper _helper = new ProcessHelper();


        public ProcessesResponse Get(GetProcessesRequest request)
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

    }
}
