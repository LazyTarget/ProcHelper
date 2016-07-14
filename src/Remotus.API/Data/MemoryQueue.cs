using System.Collections.Concurrent;
using Remotus.Base;

namespace Remotus.API.Data
{
    public class MemoryQueue<T> : IQueueEx<T>
    {
        private readonly ConcurrentQueue<T> _queue;

        public MemoryQueue()
        {
            _queue = new ConcurrentQueue<T>();
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }

        public bool TryDequeue(out T item)
        {
            var result = _queue.TryDequeue(out item);
            return result;
        }
    }
}