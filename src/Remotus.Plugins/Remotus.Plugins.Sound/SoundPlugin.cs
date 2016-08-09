using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Observables;
using Remotus.Base;
using Remotus.Base.Interfaces.Net;
using Remotus.Base.Net;
using Remotus.Plugins.Sound.Payloads;

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
        private readonly HashSet<string> _subscribedDevices = new HashSet<string>(); 
        private readonly ModelConverter _modelConverter = new ModelConverter();

        private BroadcasterBase<AudioSwitcher.AudioApi.DeviceChangedArgs> _audioDeviceChangedObserver;
        private BroadcasterBase<AudioSwitcher.AudioApi.DeviceVolumeChangedArgs> _deviceVolumeChangedObserver;
        private BroadcasterBase<AudioSwitcher.AudioApi.DevicePeakValueChangedArgs> _devicePeakValueChangedObserver;
        private BroadcasterBase<AudioSwitcher.AudioApi.DeviceMuteChangedArgs> _deviceMuteChangedObserver;
        private BroadcasterBase<AudioSwitcher.AudioApi.DeviceStateChangedArgs> _deviceStateChangedObserver;
        private BroadcasterBase<AudioSwitcher.AudioApi.DevicePropertyChangedArgs> _devicePropertyChangedObserver;


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

            InitObservers();

            Status = ServiceStatus.Initialized;
        }


        private void InitObservers()
        {
            ClearObservers();

            // Init
            _audioDeviceChangedObserver = new AsyncBroadcaster<AudioSwitcher.AudioApi.DeviceChangedArgs>();
            _audioDeviceChangedObserver.Subscribe(OnAudioDeviceChanged_OnNext, OnDeviceChanged_OnError);

            _deviceVolumeChangedObserver = new AsyncBroadcaster<AudioSwitcher.AudioApi.DeviceVolumeChangedArgs>();
            _deviceVolumeChangedObserver.Subscribe(OnDeviceVolumeChanged_OnNext, OnDeviceChanged_OnError);
            
            _devicePeakValueChangedObserver = new AsyncBroadcaster<AudioSwitcher.AudioApi.DevicePeakValueChangedArgs>();
            _devicePeakValueChangedObserver.Subscribe(OnDevicePeakValueChanged_OnNext, OnDeviceChanged_OnError);
            
            _deviceMuteChangedObserver = new AsyncBroadcaster<AudioSwitcher.AudioApi.DeviceMuteChangedArgs>();
            _deviceMuteChangedObserver.Subscribe(OnDeviceMuteChanged_OnNext, OnDeviceChanged_OnError);
            
            _deviceStateChangedObserver = new AsyncBroadcaster<AudioSwitcher.AudioApi.DeviceStateChangedArgs>();
            _deviceStateChangedObserver.Subscribe(OnDeviceStateChanged_OnNext, OnDeviceChanged_OnError);
            
            _devicePropertyChangedObserver = new AsyncBroadcaster<AudioSwitcher.AudioApi.DevicePropertyChangedArgs>();
            _devicePropertyChangedObserver.Subscribe(OnDevicePropertyChanged_OnNext, OnDeviceChanged_OnError);


            // Subscribe
            AudioController.AudioDeviceChanged.Subscribe(_audioDeviceChangedObserver);

            var devices = AudioController.GetDevices();
            foreach (var device in devices)
            {
                SubscribeOnDevice(device);
            }
        }


        private void ClearObservers()
        {
            lock (_subscribedDevices)
            {
                var count = _subscribedDevices.Count;
                _audioDeviceChangedObserver?.Dispose();
                _deviceVolumeChangedObserver?.Dispose();
                _devicePeakValueChangedObserver?.Dispose();
                _deviceStateChangedObserver?.Dispose();
                _devicePropertyChangedObserver?.Dispose();
                _deviceMuteChangedObserver?.Dispose();

                _audioDeviceChangedObserver = null;
                _deviceVolumeChangedObserver = null;
                _devicePeakValueChangedObserver = null;
                _deviceStateChangedObserver = null;
                _devicePropertyChangedObserver = null;
                _deviceMuteChangedObserver = null;

                _subscribedDevices.Clear();
                _log.Debug(() => $"Cleared device subscriptions. Count: {count}");
            }
        }



        private void SubscribeOnDevice(AudioSwitcher.AudioApi.IDevice device)
        {
            lock (_subscribedDevices)
            {
                if (_subscribedDevices.Contains(device.Id.ToString()))
                {
                    _log.Debug(() => $"Already subscribed to device '{device.Id}'");
                    return;
                }

                _log.Debug(() => $"Subscribed events for device '{device.Id}'");
                device.VolumeChanged.Subscribe(_deviceVolumeChangedObserver);
                device.PeakValueChanged.Subscribe(_devicePeakValueChangedObserver);
                device.MuteChanged.Subscribe(_deviceMuteChangedObserver);
                device.StateChanged.Subscribe(_deviceStateChangedObserver);
                device.PropertyChanged.Subscribe(_devicePropertyChangedObserver);

                _subscribedDevices.Add(device.Id.ToString());
            }
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

        
        private void OnAudioDeviceChanged_OnNext(AudioSwitcher.AudioApi.DeviceChangedArgs args)
        {
            _log.Info(() => $"OnAudioDeviceChanged() ChangedType: {args.ChangedType}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (args.ChangedType == AudioSwitcher.AudioApi.DeviceChangedType.DeviceAdded)
            {
                SubscribeOnDevice(args.Device);
            }
            else if (args.ChangedType == AudioSwitcher.AudioApi.DeviceChangedType.DeviceRemoved)
            {
                SubscribeOnDevice(args.Device);
            }
            else if (args.ChangedType == AudioSwitcher.AudioApi.DeviceChangedType.StateChanged)
            {
                SubscribeOnDevice(args.Device);
            }


            if (Status != ServiceStatus.Running)
                return;
            DeviceChangedArgs payload = _modelConverter.FromArgs(args);
            var msg = new HubMessage
            {
                Method = "OnAudioDeviceChanged",
                Args = new object[] { payload },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }

        private void OnDeviceVolumeChanged_OnNext(AudioSwitcher.AudioApi.DeviceVolumeChangedArgs args)
        {
            _log.Info(() => $"OnDeviceVolumeChanged() Volume: {args.Volume}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");
            
            if (Status != ServiceStatus.Running)
                return;
            DeviceVolumeChangedArgs payload = _modelConverter.FromArgs(args);
            var msg = new HubMessage
            {
                Method = "OnDeviceVolumeChanged",
                Args = new object[] { payload },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }

        private void OnDevicePeakValueChanged_OnNext(AudioSwitcher.AudioApi.DevicePeakValueChangedArgs args)
        {
            _log.Info(() => $"OnDevicePeakValueChanged() PeakValue: {args.PeakValue}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (Status != ServiceStatus.Running)
                return;
            DevicePeakValueChangedArgs payload = _modelConverter.FromArgs(args);
            var msg = new HubMessage
            {
                Method = "OnDevicePeakValueChanged",
                Args = new object[] { payload },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }

        private void OnDeviceMuteChanged_OnNext(AudioSwitcher.AudioApi.DeviceMuteChangedArgs args)
        {
            _log.Info(() => $"OnDeviceMuteChanged() IsMuted: {args.IsMuted}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (Status != ServiceStatus.Running)
                return;
            DeviceMuteChangedArgs payload = _modelConverter.FromArgs(args);
            var msg = new HubMessage
            {
                Method = "OnDeviceMuteChanged",
                Args = new object[] { payload },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }

        private void OnDeviceStateChanged_OnNext(AudioSwitcher.AudioApi.DeviceStateChangedArgs args)
        {
            _log.Info(() => $"OnDeviceStateChanged() State: {args.State}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (Status != ServiceStatus.Running)
                return;
            DeviceStateChangedArgs payload = _modelConverter.FromArgs(args);
            var msg = new HubMessage
            {
                Method = "OnDeviceStateChanged",
                Args = new object[] { payload },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }

        private void OnDevicePropertyChanged_OnNext(AudioSwitcher.AudioApi.DevicePropertyChangedArgs args)
        {
            _log.Info(() => $"OnDevicePropertyChanged() PropertyName: {args.PropertyName}, Device: {args.Device.FullName} [{args.Device.DeviceType}]");

            if (Status != ServiceStatus.Running)
                return;
            DevicePropertyChangedArgs payload = _modelConverter.FromArgs(args);
            var msg = new HubMessage
            {
                Method = "OnDevicePropertyChanged",
                Args = new object[] { payload },
                Queuable = false,
            };
            var task = _soundHub.InvokeCustom(msg);
            task.TryWaitAsync();
        }


        private void OnDeviceChanged_OnError(Exception error)
        {
            _log.Info(() => $"OnDeviceChanged() OnError: {error.Message}");
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
            ClearObservers();

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
