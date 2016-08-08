using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Remotus.Base;
using Remotus.Base.Interfaces.Net;
using Remotus.Base.Net;
using SpotifyAPI.Local;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyPlugin : IFunctionPlugin, IServicePlugin
    {
        private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        private ServiceStatus _status;
        //private ClientHubManager _pluginHub;
        private IExecutionContext _executionContext;
        private ICustomHubAgent _spotifyHub;

        public SpotifyPlugin()
        {
            
        }
        
        public string ID        => "79A54741-590C-464D-B1E9-0CC606771493";
        public string Name      => "Spotify";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new PlayFunction.Descriptor();
            yield return new PauseFunction.Descriptor();
            yield return new PlayUriFunction.Descriptor();
            yield return new TogglePlayingFunction.Descriptor();
            yield return new PreviousTrackFunction.Descriptor();
            yield return new NextTrackFunction.Descriptor();
            yield return new GetStatusFunction.Descriptor();
            yield return new GetProfileFunction.Descriptor();
        }

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
            _executionContext = context;
            

            ICredentials credentials = null;
            IDictionary<string, string> queryString = null;
            _spotifyHub = _executionContext?.HubAgentFactory?.CreateCustom("SpotifyHub", credentials, queryString);


            Status = ServiceStatus.Initialized;
        }


        public async Task Start()
        {
            _log.Info(() => $"Starting spotify plugin...");

            Status = ServiceStatus.Starting;

            _spotifyHub?.Connector?.ConnectContinuous();

            Worker.ConnectLocalIfNotConnected();
            Worker.LocalApi.OnPlayStateChange += LocalApi_OnPlayStateChange;
            Worker.LocalApi.OnTrackTimeChange += LocalApi_OnTrackTimeChange;
            Worker.LocalApi.OnTrackChange += LocalApi_OnTrackChange;
            Worker.LocalApi.OnVolumeChange += LocalApi_OnVolumeChange;
            Worker.LocalApi.ListenForEvents = true;

            Status = ServiceStatus.Running;
        }


        public async Task Stop()
        {
            _log.Info(() => $"Stopping spotify plugin...");

            Status = ServiceStatus.Stopping;

            Worker.LocalApi.ListenForEvents = false;
            Worker.LocalApi.OnPlayStateChange -= LocalApi_OnPlayStateChange;
            Worker.LocalApi.OnTrackTimeChange -= LocalApi_OnTrackTimeChange;
            Worker.LocalApi.OnTrackChange -= LocalApi_OnTrackChange;
            Worker.LocalApi.OnVolumeChange -= LocalApi_OnVolumeChange;
            
            // should disconnect?
            _spotifyHub?.Connector?.Disconnect();

            Status = ServiceStatus.Stopped;
        }
        

        private void LocalApi_OnPlayStateChange(object sender, PlayStateEventArgs args)
        {
            _log.Info(() => $"OnPlayStateChange() Playing: {args.Playing}");

            
            var msg = new HubMessage
            {
                Method = "OnPlayStateChange",
                Args = new[] { args },
                Queuable = true,
            };
            var task = _spotifyHub?.InvokeCustom(msg);
            task?.TryWaitAsync();
        }

        private void LocalApi_OnTrackChange(object sender, TrackChangeEventArgs args)
        {
            _log.Info(() => $"OnTrackChange() NewTrackName: {args.NewTrack?.TrackResource?.Name}");


            var msg = new HubMessage
            {
                Method = "OnTrackChange",
                Args = new[] { args },
                Queuable = true,
            };
            var task = _spotifyHub?.InvokeCustom(msg);
            task?.TryWaitAsync();
        }

        private void LocalApi_OnTrackTimeChange(object sender, TrackTimeChangeEventArgs args)
        {
            _log.Debug(() => $"OnTrackTimeChange() TrackTime: {args.TrackTime}");


            var msg = new HubMessage
            {
                Method = "OnTrackTimeChange",
                Args = new[] { args },
                Queuable = false,
            };
            var task = _spotifyHub?.InvokeCustom(msg);
            task?.TryWaitAsync();
        }

        private void LocalApi_OnVolumeChange(object sender, VolumeChangeEventArgs args)
        {
            _log.Info(() => $"OnVolumeChange() NewVolume: {args.NewVolume}");


            var msg = new HubMessage
            {
                Method = "OnVolumeChange",
                Args = new[] { args },
                Queuable = true,
            };
            var task = _spotifyHub?.InvokeCustom(msg);
            task?.TryWaitAsync();
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
            _spotifyHub?.Connector?.Disconnect();
            _spotifyHub?.Connector?.Dispose();
            _spotifyHub?.Dispose();
            _spotifyHub = null;
        }
    }
}
