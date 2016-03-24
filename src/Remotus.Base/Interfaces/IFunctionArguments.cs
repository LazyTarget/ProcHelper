namespace Remotus.Base
{
    public interface IFunctionArguments
    {
        IFunctionDescriptor Descriptor { get; set; }
        IParameterCollection Parameters { get; set; }
    }
}