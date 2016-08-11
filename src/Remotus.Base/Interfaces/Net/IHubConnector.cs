using System;
using System.Threading.Tasks;

namespace Remotus.Base.Interfaces.Net
{
    public interface IHubConnector : IDisposable
    {
        event EventHandler<HubConnectionStateChange> StateChanged; 
        string ConnectionId { get; }
        bool Connected { get; }
        ConnectionState State { get; }
        Task Connect();
        bool EnsureReconnecting();
        void Disconnect();
    }
}
