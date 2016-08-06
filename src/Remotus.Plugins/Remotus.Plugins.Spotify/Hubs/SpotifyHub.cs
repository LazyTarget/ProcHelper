using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Remotus.API.Hubs;

namespace Remotus.Plugins.Spotify
{
    public class SpotifyHub : HubBase
    {
        public SpotifyHub()
        {

        }

        public override string HubName => MethodBase.GetCurrentMethod().DeclaringType.Name;


        public override async Task OnConnected()
        {
            await base.OnConnected();
        }

    }
}