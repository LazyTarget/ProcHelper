using System;
using System.Drawing;
using Newtonsoft.Json;

namespace FullCtrl.Base
{
    public class AudioSession
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public float Volume { get; set; }
        public bool Muted { get; set; }
        [JsonConverter(typeof(BitmapConverter))]
        public Bitmap Icon { get; set; }
        public Guid GroupingParam { get; set; }
    }
}
