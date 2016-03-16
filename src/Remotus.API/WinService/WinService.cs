namespace FullCtrl.API
{
    /// <summary>
    /// Class exposed for Windows Service
    /// </summary>
    public class WinService : System.ServiceProcess.ServiceBase
    {
        public const string ServiceName = "FullCtrl.API";
        public const string DisplayName = "FullCtrl API";
        public const string Description = "Windows Service which can take requests for managing processes and services";

        internal ApiService ApiService { get; private set; }


        public WinService()
        {
            ApiService = new ApiService();
        }
        
        internal WinService(ApiService apiService)
        {
            ApiService = apiService;
        }


        protected override void OnStart(string[] args)
        {
            ApiService.Start(args);
            base.OnStart(args);
        }

        protected override void OnPause()
        {
            //ApiService.Pause();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            //ApiService.Continue();
            base.OnContinue();
        }

        protected override void OnStop()
        {
            ApiService.Stop();
            base.OnStop();
        }

    }
}
