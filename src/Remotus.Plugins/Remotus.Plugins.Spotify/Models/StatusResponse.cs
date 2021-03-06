﻿using Newtonsoft.Json;
using SpotifyAPI.Local.Models;

namespace Remotus.Plugins.Spotify
{
    public class StatusResponse
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("client_version")]
        public string ClientVersion { get; set; }

        [JsonProperty("playing")]
        public bool Playing { get; set; }

        [JsonProperty("schuffle")]
        public bool Shuffle { get; set; }

        [JsonProperty("repeat")]
        public bool Repeat { get; set; }

        [JsonProperty("play_enabled")]
        public bool PlayEnabled { get; set; }

        [JsonProperty("prev_enabled")]
        public bool PrevEnabled { get; set; }

        [JsonProperty("next_enabled")]
        public bool NextEnabled { get; set; }

        [JsonProperty("track")]
        public Track Track { get; set; }

        [JsonProperty("playing_position")]
        public double PlayingPosition { get; set; }

        [JsonProperty("server_time")]
        public int ServerTime { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("running")]
        public bool Running { get; set; }
    }
}
