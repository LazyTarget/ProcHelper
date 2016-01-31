namespace ProcHelper
{
    public partial class HttpService
    {
        public WinServicesResponse Any(GetWinServicesRequest request)
        {
            var response = _worker.GetWinServices(request);
            return response;
        }


        public StartWinServiceResponse Any(StartWinServiceRequest request)
        {
            var response = _worker.StartWinService(request);
            return response;
        }


        public PauseWinServiceResponse Any(PauseWinServiceRequest request)
        {
            var response = _worker.PauseWinService(request);
            return response;
        }


        public ContinueWinServiceResponse Any(ContinueWinServiceRequest request)
        {
            var response = _worker.ContinueWinService(request);
            return response;
        }


        public StopWinServiceResponse Any(StopWinServiceRequest request)
        {
            var response = _worker.StopWinService(request);
            return response;
        }
    }
}
