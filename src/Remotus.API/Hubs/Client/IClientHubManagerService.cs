namespace Remotus.API.Hubs.Client
{
    public interface IClientHubManagerService
    {
        void Register(ClientHubManager manager);
        void Unregister(ClientHubManager manager);
    }
}