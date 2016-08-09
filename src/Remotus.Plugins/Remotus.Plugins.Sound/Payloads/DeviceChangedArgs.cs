namespace Remotus.Plugins.Sound.Payloads
{
    public class DeviceChangedArgs
    {
        public DeviceChangedType ChangeType { get; set; }
        public AudioDevice Device { get; set; }
    }

    public class DeviceVolumeChangedArgs : DeviceChangedArgs
    {
        public double Volume { get; set; }
    }

    public class DevicePeakValueChangedArgs : DeviceChangedArgs
    {
        public double PeakValue { get; set; }
    }

    public class DeviceMuteChangedArgs : DeviceChangedArgs
    {
        public bool IsMuted { get; set; }
    }

    public class DeviceStateChangedArgs : DeviceChangedArgs
    {
        public AudioDeviceState State { get; set; }
    }

    public class DevicePropertyChangedArgs : DeviceChangedArgs
    {
        public string PropertyName { get; set; }
    }
}
