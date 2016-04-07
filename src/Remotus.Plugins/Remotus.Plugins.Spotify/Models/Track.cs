using System;
using System.Drawing;
using Newtonsoft.Json;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class Track
    {
        [JsonProperty("length")]
        public int Length { get; set; }
        [JsonProperty("track_type")]
        public string TrackType { get; set; }
        [JsonProperty("is_ad")]
        public bool IsAd { get; set; }

        [JsonProperty("artist_resource")]
        public SpotifyResource ArtistResource { get; set; }

        [JsonProperty("album_resource")]
        public SpotifyResource AlbumResource { get; set; }

        [JsonProperty("track_resource")]
        public SpotifyResource TrackResource { get; set; }

        [JsonProperty("album_art")]
        [JsonConverter(typeof(BitmapConverter))]
        public Bitmap AlbumArt { get; set; }
    }
}
