using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyPlugin : IFunctionPlugin
    {
        public string ID        => "79A54741-590C-464D-B1E9-0CC606771493";
        public string Name      => nameof(SpotifyPlugin);
        public string Version   => "1.0.0.0";

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            yield return new PlayFunction.Descriptor();
            yield return new PauseFunction.Descriptor();
        }
    }
}
