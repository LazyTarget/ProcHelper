using System;
using System.Drawing;
using CoreAudioApi;
using Newtonsoft.Json;

namespace FullCtrl.Base
{
    public class AudioDevice
    {
        public string ID { get; set; }
        public string FriendlyName { get; set; }
        public bool Muted { get; set; }
        public float Volume { get; set; }
        public EDeviceState State { get; set; }
        public bool DefaultDevice { get; set; }
    }
}
