using System;
using System.Configuration;

namespace Remotus.Plugins.Steam
{
    public class SteamActivityPollerConfigSection : ConfigurationSection
    {
        public static SteamActivityPollerConfigSection LoadFromConfig()
        {
            var settings = ConfigurationManager.GetSection("SteamActivityPoller") as SteamActivityPollerConfigSection;
            if (settings == null)
                throw new ApplicationException("Could not load settings");
            return settings;
        }
        

        [ConfigurationProperty("Settings")]
        public SteamActivityPollerSettingsConfigElement Settings
        {
            get { return (SteamActivityPollerSettingsConfigElement) this["Settings"]; }
        }

    }
}
