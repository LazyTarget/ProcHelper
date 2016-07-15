using System.Collections.Concurrent;
using Remotus.Base;

namespace Remotus.API.Data
{
    public class MessageMemoryCache : IMessageCache
    {
        private readonly ConcurrentQueue<IHubMessage> _queue;

        public MessageMemoryCache()
        {
            _queue = new ConcurrentQueue<IHubMessage>();
        }

        public void Enqueue(IHubMessage item)
        {
            _queue.Enqueue(item);
        }

        public bool TryDequeue(out IHubMessage item)
        {
            var result = _queue.TryDequeue(out item);
            return result;
        }
    }
}