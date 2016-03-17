namespace Remotus.Base
{
    public interface IFunctionDescriptor
    {
        string Name { get; }
        // todo: more metadata, such as logo

        IParameterCollection GetParameters();
        IFunction Instantiate();
    }
}