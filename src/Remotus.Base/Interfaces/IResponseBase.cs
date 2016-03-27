using System;

namespace Remotus.Base
{
    public interface IResponseBase
    {
        IError Error { get; }
        object Result { get; }
        Type ResultType { get; }
    }

    public interface IResponseBase<out TResult> : IResponseBase
    {
        new TResult Result { get; }
    }
}
