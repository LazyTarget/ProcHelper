using Remotus.Base;

namespace Remotus.Plugins.Spotify.Hub
{
    public class OnTrackChangeHubTrigger : HubTrigger
    {
        public OnTrackChangeHubTrigger()
        {
            Parameters = new ParametersDef();
        }

        public override string HubName
        {
            get { return "SpotifyHub"; }
            protected set { }
        }

        public override string EventName
        {
            get { return "OnTrackChange"; }
            protected set { }
        }

        public override IParameterCollection Parameters { get; protected set; }
        

        public class ParametersDef : ParameterCollection
        {
            public ParametersDef()
            {
                OldTrack = new Parameter<Track>
                {
                    Name = ParameterKeys.OldTrack,
                    Required = false,
                    Type = typeof(Track),
                    Value = null,
                };
                NewTrack = new Parameter<Track>
                {
                    Name = ParameterKeys.NewTrack,
                    Required = true,
                    Type = typeof(Track),
                    Value = null,
                };
            }

            public IParameter<Track> OldTrack
            {
                get { return this.GetOrDefault<Track>(ParameterKeys.OldTrack); }
                private set { this[ParameterKeys.OldTrack] = value; }
            }

            public IParameter<Track> NewTrack
            {
                get { return this.GetOrDefault<Track>(ParameterKeys.NewTrack); }
                private set { this[ParameterKeys.NewTrack] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string OldTrack = "OldTrack";
            public const string NewTrack = "NewTrack";
        }
    }
}
