using FullCtrl.API.Interfaces;
using Newtonsoft.Json;

namespace FullCtrl.API.Models
{
    public class Link : ILink
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "relative")]
        public string Relative { get; set; }
    }
}
