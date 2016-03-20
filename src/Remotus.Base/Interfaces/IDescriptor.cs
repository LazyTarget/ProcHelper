namespace Remotus.Base
{
    public interface IComponentDescriptor
    {
        string ID { get; }
        string Name { get; }
        string Version { get; }
    }
}
