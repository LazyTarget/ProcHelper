using System.Collections.Concurrent;
using Remotus.Base;
using Remotus.Base.Models.Hub;

namespace Remotus.API.Data
{
    public class MessageMemoryCache : IMessageCache
    {
        private readonly ConcurrentQueue<HubRequest> _queue;

        public MessageMemoryCache()
        {
            _queue = new ConcurrentQueue<HubRequest>();
        }

        public void Enqueue(HubRequest item)
        {
            _queue.Enqueue(item);
        }

        public bool TryDequeue(out HubRequest item)
        {
            var result = _queue.TryDequeue(out item);
            return result;
        }
    }
}