using Remotus.Base.Net;

namespace Remotus.Base
{
    public interface IMessageCache
    {
        //int Count { get; }
        bool Any();
        void Enqueue(HubRequest item);
        bool TryDequeue(out HubRequest item);
    }
}
