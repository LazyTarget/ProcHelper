namespace FullCtrl.Base
{
    public interface IFunctionResult : IResponseBase
    {
        //object Result { get; }
        //IError Error { get; }
        IFunctionArguments Arguments { get; }
    }

    public interface IFunctionResult<TResult> : IFunctionResult, IResponseBase<TResult>
    {
        
    }
}