using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Observables;
using Remotus.Base;
using Remotus.Base.Interfaces.Net;
using Remotus.Base.Net;

namespace Remotus.Plugins.Sound
{
    public class SoundPlugin : IFunctionPlugin, IServicePlugin
    {
        private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod().DeclaringType.FullName);

        private static Lazy<CoreAudioController> _audioController =
            new Lazy<CoreAudioController>(() => new CoreAudioController());

        internal static readonly CoreAudioController AudioController = _audioController?.Value;


        private ServiceStatus _status;
        private IExecutionContext _executionContext;
        private ICustomHubAgent _soundHub;
        private BroadcasterBase<DeviceChangedArgs> _audioDeviceChangedObserver;
        private BroadcasterBase<DeviceChangedArgs> _playbackDeviceVolumeChangedObserver;


        public SoundPlugin()
        {
            
        }


        public string ID        => "ABA6417A-65A2-4761-9B01-AA9DFFC074C0";
        public string Name      => "Sound";
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new GetAudioDevicesFunction.Descriptor();
            yield return new GetAudioSessionsFunction.Descriptor();
            yield return new SetDefaultAudioDeviceFunction.Descriptor();
            yield return new SetDefaultAudioCommunicationsDeviceFunction.Descriptor();
            yield return new SetAudioDeviceVolumeFunction.Descriptor();
            yield return new SetAudioDeviceMutedFunction.Descriptor();
            yield return new ToggleAudioDeviceMutedFunction.Descriptor();
            yield return new SetAudioSessionVolumeFunction.Descriptor();
            yield return new SetAudioSessionMutedFunction.Descriptor();
            yield return new ToggleAudioSessionMutedFunction.Descriptor();
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
            _soundHub = _executionContext?.HubAgentFactory?.CreateCustom("SoundHub", credentials, queryString);
            
            _audioDeviceChangedObserver?.Dispose();
            _audioDeviceChangedObserver = new AsyncBroadcaster<DeviceChangedArgs>();
            _audioDeviceChangedObserver.Subscribe(OnAudioDeviceChanged_OnNext);
            AudioController.AudioDeviceChanged.Subscribe(_audioDeviceChangedObserver);


            _playbackDeviceVolumeChangedObserver?.Dispose();
            _playbackDeviceVolumeChangedObserver = new AsyncBroadcaster<DeviceChangedArgs>();
            _playbackDeviceVolumeChangedObserver.Subscribe(OnPlaybackDeviceVolumeChanged_OnNext);
            AudioController.DefaultPlaybackDevice.VolumeChanged.Subscribe(_playbackDeviceVolumeChangedObserver);

            Status = ServiceStatus.Initialized;
        }


        public async Task Start()
        {
            _log.Info(() => $"Starting sound plugin...");

            Status = ServiceStatus.Starting;

            _soundHub?.Connector?.ConnectContinuous();

            Status = ServiceStatus.Running;
        }


        public async Task Stop()
        {
            _log.Info(() => $"Stopping sound plugin...");

            Status = ServiceStatus.Stopping;
            
            
            // should disconnect?
            _soundHub?.Connector?.Disconnect();

            Status = ServiceStatus.Stopped;
        }

        
        private void OnAudioDeviceChanged_OnNext(DeviceChangedArgs args)
        {
            _log.Info(() => $"OnAudioDeviceChanged() ChangedType: {args.ChangedType}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (Status != ServiceStatus.Running)
                return;

            var msg = new HubMessage
            {
                Method = "OnAudioDeviceChanged",
                Args = new object[] { args },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }

        private void OnPlaybackDeviceVolumeChanged_OnNext(DeviceChangedArgs args)
        {
            _log.Info(() => $"OnPlaybackDeviceVolumeChanged() ChangedType: {args.ChangedType}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (Status != ServiceStatus.Running)
                return;

            var msg = new HubMessage
            {
                Method = "OnPlaybackDeviceVolumeChanged",
                Args = new object[] { args },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
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
            _audioDeviceChangedObserver.Dispose();


            if (_audioController.IsValueCreated)
            {
                _audioController.Value?.Dispose();
            }
            _audioController = new Lazy<CoreAudioController>(() => null);

            _soundHub?.Connector?.Disconnect();
            _soundHub?.Connector?.Dispose();
            _soundHub?.Dispose();
            _soundHub = null;
        }
    }
}
