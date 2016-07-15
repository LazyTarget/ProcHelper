using Remotus.Base.Models.Hub;

namespace Remotus.Base
{
    public interface IMessageCache
    {
        //int Count { get; }
        void Enqueue(HubRequest item);
        bool TryDequeue(out HubRequest item);
    }
}
