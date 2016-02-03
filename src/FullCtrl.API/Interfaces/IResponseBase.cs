using System.Collections.Generic;

namespace FullCtrl.API.Interfaces
{
    public interface IResponseBase<TResult>
    {
        IDictionary<string, ILink> Links { get; }

        object Request { get; }

        TResult Result { get; }
    }
}
