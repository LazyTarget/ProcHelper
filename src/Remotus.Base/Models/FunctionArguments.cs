namespace Remotus.Base
{
    public class FunctionArguments : IFunctionArguments
    {
        public FunctionArguments()
        {
            Parameters = new ParameterCollection();
        }

        public IFunctionDescriptor Descriptor { get; set; }
        public IParameterCollection Parameters { get; set; }
    }
}