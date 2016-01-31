namespace ProcHelper
{
    public partial class HttpService
    {
        public ProcessesResponse Any(GetProcessesRequest request)
        {
            var response = _worker.GetProcesses(request);
            return response;
        }


        public StartProcessResponse Any(StartProcessRequest request)
        {
            var response = _worker.StartProcess(request);
            return response;
        }


        public KillProcessResponse Any(KillProcessRequest request)
        {
            var response = _worker.KillProcess(request);
            return response;
        }
    }
}
