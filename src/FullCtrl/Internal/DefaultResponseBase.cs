using System.Collections.Generic;
using FullCtrl.Base;

namespace FullCtrl.Internal
{
    public class DefaultResponseBase<TResult> : IResponseBase<TResult>
    {
        public DefaultResponseBase()
        {
            Links = new Dictionary<string, ILink>();
        } 

        public IDictionary<string, ILink> Links { get; }
        public IError Error { get; set; }
        public TResult Result { get; set; }

        object IResponseBase.Result
        {
            get { return Result; }
        }
    }
}
