namespace FullCtrl.Base
{
    public class FunctionResult : IFunctionResult
    {
        public IFunctionArguments Arguments { get; set; }
        public IError Error { get; set; }
        public object Result { get; set; }
    }

    public class FunctionResult<TResult> : FunctionResult, IFunctionResult<TResult>
    {
        public new TResult Result
        {
            get { return (TResult)base.Result; }
            set { base.Result = value; }
        }
    }
}