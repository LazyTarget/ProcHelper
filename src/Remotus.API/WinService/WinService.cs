namespace Remotus.API.WinService
{
    /// <summary>
    /// Class exposed for Windows Service
    /// </summary>
    public class WinService : System.ServiceProcess.ServiceBase
    {
        public const string ServiceName = "Remotus";
        public const string DisplayName = "Remotus";
        public const string Description = "Windows Service which can take requests for managing processes and services";

        private ServiceInstance _serviceInstance;


        public WinService()
        {
            _serviceInstance = new ServiceInstance();
        }
        
        internal WinService(ServiceInstance serviceInstance)
        {
            _serviceInstance = serviceInstance;
        }


        protected override void OnStart(string[] args)
        {
            _serviceInstance?.Start(args);

            base.OnStart(args);
        }

        protected override void OnPause()
        {
            //ApiService?.Pause();
            base.OnPause();
        }

        protected override void OnContinue()
        {
            //ApiService?.Continue();

            base.OnContinue();
        }

        protected override void OnStop()
        {
            _serviceInstance?.Stop();

            base.OnStop();
        }

        protected override void Dispose(bool disposing)
        {
            _serviceInstance?.Dispose();
            _serviceInstance = null;

            base.Dispose(disposing);
        }
    }
}
