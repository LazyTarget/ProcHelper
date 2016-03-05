using System;
using System.Drawing;
using Newtonsoft.Json;

namespace FullCtrl.Base
{
    public class AudioSession
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Volume { get; set; }
        public bool Muted { get; set; }
        public Guid DeviceID { get; set; }
        public bool IsSystemSession { get; set; }

        public string IconPath { get; set; }
        [JsonConverter(typeof(BitmapConverter))]
        public Bitmap IconRaw { get; set; }
    }
}
