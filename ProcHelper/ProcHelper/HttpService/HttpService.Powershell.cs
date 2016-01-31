namespace ProcHelper
{
    public partial class HttpService
    {
        public PowershellResponse Any(PowershellFileRequest request)
        {
            var response = _worker.RunPowershellFile(request);
            return response;
        }


        public PowershellResponse Any(PowershellQueryRequest request)
        {
            var response = _worker.RunPowershellQuery(request);
            return response;
        }
    }
}
