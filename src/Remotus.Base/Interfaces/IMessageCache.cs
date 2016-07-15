namespace Remotus.Base
{
    public interface IMessageCache
    {
        //int Count { get; }
        void Enqueue(IHubMessage item);
        bool TryDequeue(out IHubMessage item);
    }
}
