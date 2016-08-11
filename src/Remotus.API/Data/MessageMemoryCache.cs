using System.Collections.Concurrent;
using Remotus.Base;
using Remotus.Base.Net;

namespace Remotus.API.Data
{
    public class MessageMemoryCache : IMessageCache
    {
        private readonly ConcurrentQueue<HubRequest> _queue;

        public MessageMemoryCache()
        {
            _queue = new ConcurrentQueue<HubRequest>();
        }

        public bool Any()
        {
            var result = !_queue.IsEmpty;
            return result;
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