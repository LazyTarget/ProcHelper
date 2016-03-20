namespace Remotus.Base
{
    public interface IFunctionDescriptor : IComponentDescriptor, IComponentInstantiator<IFunction>
    {
        //string Name { get; }
        // todo: more metadata, such as logo

        IParameterCollection GetParameters();
        //IFunction Instantiate();
    }
}