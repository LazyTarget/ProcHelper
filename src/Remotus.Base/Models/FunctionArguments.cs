namespace Remotus.Base
{
    public class FunctionArguments : IFunctionArguments
    {
        public FunctionArguments()
        {
            Parameters = new ParameterCollection();
        }

        public IParameterCollection Parameters { get; set; }
    }
}