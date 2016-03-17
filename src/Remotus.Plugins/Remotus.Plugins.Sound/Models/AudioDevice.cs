using System;
using System.Drawing;
using Newtonsoft.Json;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class AudioDevice
    {
        public Guid ID { get; set; }
        public string FriendlyName { get; set; }
        public string InterfaceName { get; set; }
        public bool Muted { get; set; }
        public double Volume { get; set; }
        public AudioDeviceState State { get; set; }
        public AudioDeviceType DeviceType { get; set; }
        public bool DefaultDevice { get; set; }
        public bool IsDefaultCommunicationsDevice { get; set; }

        public AudioDeviceIcon DeviceIconType { get; set; }
        public string IconPath { get; set; }
        [JsonConverter(typeof(BitmapConverter))]
        public Bitmap IconRaw { get; set; }
    }
}
