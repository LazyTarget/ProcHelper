using System;

namespace FullCtrl.Base
{
    [Flags]
    public enum AudioDeviceType
    {
        Playback = 1,
        Capture = 2,
        All = 3
    }

    [Flags]
    public enum AudioDeviceState
    {
        Active = 1,
        Disabled = 2,
        NotPresent = 4,
        Unplugged = 8,
        All = 15
    }

    public enum AudioDeviceIcon
    {
        DesktopMicrophone = 0,
        Digital = 1,
        Headphones = 2,
        Headset = 3,
        Kinect = 4,
        LineIn = 5,
        Phone = 6,
        Speakers = 7,
        StereoMix = 8,
        Monitor = 9,
        Unknown = 10
    }
}
