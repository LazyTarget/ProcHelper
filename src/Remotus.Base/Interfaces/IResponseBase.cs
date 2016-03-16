namespace FullCtrl.Base
{
    public interface IResponseBase
    {
        object Result { get; }
        IError Error { get; }
    }

    public interface IResponseBase<TResult> : IResponseBase
    {
        new TResult Result { get; }
    }
}
