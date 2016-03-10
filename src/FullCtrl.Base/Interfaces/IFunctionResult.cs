namespace FullCtrl.Base
{
    public interface IFunctionResult
    {
        IFunctionArguments Arguments { get; }
        IError Error { get; }
        object Result { get; }
    }
}