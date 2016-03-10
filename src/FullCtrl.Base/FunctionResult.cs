namespace FullCtrl.Base
{
    public class FunctionResult : IFunctionResult
    {
        public IFunctionArguments Arguments { get; set; }
        public IError Error { get; set; }
    }
}