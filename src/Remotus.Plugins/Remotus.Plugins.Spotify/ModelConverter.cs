using System.Drawing;
using SpotifyAPI.Local.Enums;

namespace Remotus.Plugins.Spotify
{
    public class ModelConverter
    {
        public virtual StatusResponse FromStatusResponse(SpotifyAPI.Local.Models.StatusResponse state)
        {
            if (state == null)
                return null;

            var result = new StatusResponse
            {
                Track = FromTrack(state.Track),
                Online = state.Online,
                ClientVersion = state.ClientVersion,
                PlayEnabled = state.PlayEnabled,
                NextEnabled = state.NextEnabled,
                PrevEnabled = state.PrevEnabled,
                Playing = state.Playing,
                PlayingPosition = state.PlayingPosition,
                Repeat = state.Repeat,
                Shuffle = state.Shuffle,
                Running = state.Running,
                ServerTime = state.ServerTime,
                Version = state.Version,
                Volume = state.Volume,
            };
            return result;
        }

        public virtual Track FromTrack(SpotifyAPI.Local.Models.Track state)
        {
            if (state == null)
                return null;

            var result = new Track
            {
                Length = state.Length,
                TrackType = state.TrackType,
                IsAd = state.IsAd(),
                ArtistResource = FromSpotifyResource(state.ArtistResource),
                AlbumResource = FromSpotifyResource(state.AlbumResource),
                TrackResource = FromSpotifyResource(state.TrackResource),
            };

            var albumArtSize = AlbumArtSize.Size320;
            //result.AlbumArt = state.GetAlbumArt(albumArtSize);
            var bytes = state.GetAlbumArtAsByteArray(albumArtSize); // todo: arg for wheter to load images?
            using (var stream = new System.IO.MemoryStream(bytes, false))
                result.AlbumArt = new Bitmap(stream);
            return result;
        }

        public virtual SpotifyResource FromSpotifyResource(SpotifyAPI.Local.Models.SpotifyResource state)
        {
            if (state == null)
                return null;
            
            var result = new SpotifyResource
            {
                Name = state.Name,
                Uri = state.Uri,
            };
            return result;
        }
    }
}
