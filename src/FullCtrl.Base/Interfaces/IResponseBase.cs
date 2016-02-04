using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface IResponseBase
    {
        IDictionary<string, ILink> Links { get; }

        IError Error { get; }

        object Result { get; }
    }

    public interface IResponseBase<TResult> : IResponseBase
    {
        new TResult Result { get; }
    }
}
