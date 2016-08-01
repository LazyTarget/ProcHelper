using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Models.Hub;
using Remotus.Base.Models.Payloads;

namespace Remotus.Plugins.Steam
{
    public class SteamPlugin : IServicePlugin
    {
        private PollingEngine.Core.ProgramManager _programManager;
        private PollingEngine.Core.PollingContext _pollingContext;
        private SteamActivityPoller _activityPoller;
        private ServiceStatus _status;


        public string ID        => "A0113EE0-E670-44E3-8F73-71FCEE22A357";
        public string Name      => "Steam";
        public string Version   => "1.0.0.0";
        
        public ServiceStatus Status
        {
            get { return _status; }
            private set
            {
                var old = _status;
                if (old != value)
                {
                    _status = value;
                    var e = new ServiceStatusChangedEventArgs(old, value);
                    InvokeOnStatusChanged(e);
                }
            }
        }

        public event EventHandler<ServiceStatusChangedEventArgs> OnStatusChanged;


        public async Task Init(IExecutionContext context)
        {
            if (Status != ServiceStatus.None &&
                Status != ServiceStatus.Stopped)
            {
                // Already initialized
                return;
            }

            Status = ServiceStatus.Initializing;



            var interval = TimeSpan.FromSeconds(30);
            _activityPoller = new SteamActivityPoller();
            _pollingContext = new PollingEngine.Core.PollingContext(_activityPoller, interval);

            _programManager = new PollingEngine.Core.ProgramManager();
            _programManager.Load(new [] { _pollingContext });
            

            Status = ServiceStatus.Initialized;
        }


        public async Task Start()
        {
            Status = ServiceStatus.Starting;

            _programManager.Start();

            Status = ServiceStatus.Running;
        }


        public async Task Stop()
        {
            Status = ServiceStatus.Stopping;

            var force = false;
            _programManager.Exit(force);

            Status = ServiceStatus.Stopped;
        }

        protected virtual void InvokeOnStatusChanged(ServiceStatusChangedEventArgs e)
        {
            try
            {
                // todo: Run in seperate thread?
                OnStatusChanged?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                
            }
        }


        public void Dispose()
        {

        }
    }
}
