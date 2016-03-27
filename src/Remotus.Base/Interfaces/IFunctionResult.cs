namespace Remotus.Base
{
    public interface IFunctionResult : IResponseBase
    {
        //object Result { get; }
        //IError Error { get; }
        IFunctionArguments Arguments { get; }
    }

    public interface IFunctionResult<out TResult> : IFunctionResult, IResponseBase<TResult>
    {
        
    }
}