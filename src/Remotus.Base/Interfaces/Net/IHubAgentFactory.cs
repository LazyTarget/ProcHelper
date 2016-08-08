using System.Collections.Generic;
using System.Net;

namespace Remotus.Base
{
    public interface IHubAgentFactory
    {
        IHubAgent Create(string hubName, ICredentials credentials, IDictionary<string, string> queryString = null);
        IHubAgentManager Create(IEnumerable<string> hubNames, ICredentials credentials, IDictionary<string, string> queryString = null);

        ICustomHubAgent CreateCustom(string hubName, ICredentials credentials, IDictionary<string, string> queryString = null);
    }
}
