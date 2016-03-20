namespace Remotus.Base
{
    public interface IComponentInstantiator<out TComponent>
    {
        TComponent Instantiate();
    }
}
