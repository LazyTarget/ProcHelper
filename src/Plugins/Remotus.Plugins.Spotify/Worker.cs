namespace Remotus.Plugins.Spotify
{
    public static class Worker
    {
        private static bool _connected;
        public static readonly SpotifyAPI.Local.SpotifyLocalAPI Api;

        static Worker()
        {
            Api = new SpotifyAPI.Local.SpotifyLocalAPI();
            Api.ListenForEvents = false;        // todo: Implement as IServicePlugin, and expose event hooks
        }


        public static bool ConnectIfNotConnected()
        {
            if (!_connected)
            {
                _connected = Api.Connect();
            }
            return _connected;
        }


        public static void Reset()
        {
            _connected = false;
        }



    }
}
