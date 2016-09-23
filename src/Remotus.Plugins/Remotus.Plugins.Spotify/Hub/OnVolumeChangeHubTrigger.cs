using Remotus.Base;

namespace Remotus.Plugins.Spotify.Hub
{
    public class OnVolumeChangeHubTrigger : HubTrigger
    {
        public OnVolumeChangeHubTrigger()
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
            get { return "OnVolumeChange"; }
            protected set { }
        }

        public override IParameterCollection Parameters { get; protected set; }
        

        public class ParametersDef : ParameterCollection
        {
            public ParametersDef()
            {
                OldTrack = new Parameter<double>
                {
                    Name = ParameterKeys.OldVolume,
                    Required = false,
                    Type = typeof(double),
                };
                NewTrack = new Parameter<double>
                {
                    Name = ParameterKeys.NewVolume,
                    Required = true,
                    Type = typeof(double),
                };
            }

            public IParameter<double> OldTrack
            {
                get { return this.GetOrDefault<double>(ParameterKeys.OldVolume); }
                private set { this[ParameterKeys.OldVolume] = value; }
            }

            public IParameter<double> NewTrack
            {
                get { return this.GetOrDefault<double>(ParameterKeys.NewVolume); }
                private set { this[ParameterKeys.NewVolume] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string OldVolume = "OldVolume";
            public const string NewVolume = "NewVolume";
        }
    }
}
