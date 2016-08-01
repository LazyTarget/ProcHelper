using System.Configuration;

namespace Remotus.Plugins.Steam
{
    public class SteamIDConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("SteamID", IsRequired = true)]
        public long SteamID
        {
            get { return (long)this["SteamID"]; }
            set { this["SteamID"] = value; }
        }
    }
}
