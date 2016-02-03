using System.Collections.Generic;

namespace FullCtrl.API.Interfaces
{
    public interface IResponseBase<TResult>
    {
        IDictionary<string, ILink> Links { get; }

        TResult Result { get; }
    }
}
