namespace ProcHelper
{
    /// <summary>
    /// Class exposed for Windows Service
    /// </summary>
    public class WinService : System.ServiceProcess.ServiceBase
    {
        public const string ServiceName = "ProcHelper";
        public const string DisplayName = "ProcHelper";
        public const string Description = "Windows Service which can take requests for managing processes and services";

        internal WorkerService WorkerService { get; private set; }


        public WinService()
        {
            WorkerService = new WorkerService();
        }
        
        internal WinService(WorkerService workerService)
        {
            WorkerService = workerService;
        }


        protected override void OnStart(string[] args)
        {
            WorkerService.Start(args);
            base.OnStart(args);
        }

        protected override void OnPause()
        {
            //WorkerService.Pause();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            //WorkerService.Continue();
            base.OnContinue();
        }

        protected override void OnStop()
        {
            WorkerService.Stop();
            base.OnStop();
        }

    }
}
