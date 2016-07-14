namespace Remotus.Base
{
    public interface IQueueEx<T>
    {
        //int Count { get; }
        void Enqueue(T item);
        bool TryDequeue(out T item);
    }
}
