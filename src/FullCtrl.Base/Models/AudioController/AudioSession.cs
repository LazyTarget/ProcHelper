using System;
using System.Drawing;
using Newtonsoft.Json;

namespace FullCtrl.Base
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AudioSession
    {
        [JsonProperty]
        public int Volume { get; set; }
        [JsonProperty]
        public bool Muted { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(BitmapConverter))]
        public Bitmap Icon { get; set; }
        [JsonProperty]
        public string ID { get; set; }
        [JsonProperty]
        public Guid GroupingParam { get; set; }
    }
}
