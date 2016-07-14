namespace Remotus.Base
{
    public interface IHubAgentFactory
    {
        IHubAgent Create(string[] hubs);
    }
}
