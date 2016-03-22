using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyPlugin : ICommandPlugin
    {
        public string ID        => "79A54741-590C-464D-B1E9-0CC606771493";
        public string Name      => nameof(SpotifyPlugin);
        public string Version   => "1.0.0.0";

        public IEnumerable<ICommandDescriptor> GetCommands()
        {
            yield return new PausePlaybackCommand.Descriptor();
        }
    }
}
