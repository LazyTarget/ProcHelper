using System.Collections.Generic;

namespace Remotus.Base
{
    public interface IHubPlugin : IPlugin
    {
        IEnumerable<IHubDescriptor> GetHubs();
    }
}
