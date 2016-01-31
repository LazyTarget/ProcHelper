namespace ProcHelper
{
    public class HttpService : ServiceStack.Service
    {
        private Worker _worker = new Worker();


        #region ProcHelper

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

        #endregion


        #region Powershell

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

        #endregion


        #region WinServiceHelper

        /*
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
        */

        #endregion


        #region InputHelper

        public MoveMouseResponse Any(GetMousePositionRequest request)
        {
            var response = _worker.GetMousePosition(request);
            return response;
        }

        public MoveMouseResponse Any(MoveMouseBy request)
        {
            var response = _worker.MoveMouseBy(request);
            return response;
        }

        public MoveMouseResponse Any(MoveMouseTo request)
        {
            var response = _worker.MoveMouseTo(request);
            return response;
        }

        public MoveMouseResponse Any(MoveMouseToPositionOnVirtualDesktop request)
        {
            var response = _worker.MoveMouseToPositionOnVirtualDesktop(request);
            return response;
        }

        #endregion

    }
}
