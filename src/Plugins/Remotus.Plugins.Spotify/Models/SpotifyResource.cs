using Newtonsoft.Json;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyResource
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        //[JsonProperty("location")]
        //public TrackResourceLocation Location { get; set; }
    }
}