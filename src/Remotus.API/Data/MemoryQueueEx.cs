using System.Collections.Concurrent;

namespace Remotus.API.Data
{
    public class MemoryQueueEx<T> : IQueueEx<T>
    {
        private readonly ConcurrentQueue<T> _queue;

        public MemoryQueueEx()
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