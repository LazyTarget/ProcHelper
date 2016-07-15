using System.Net;

namespace Remotus.Base
{
    public interface IHubAgentFactory
    {
        IHubAgent Create(string hubName, ICredentials credentials);
    }
}
