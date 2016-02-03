using System.Collections.Generic;
using FullCtrl.API.Interfaces;

namespace FullCtrl.API.Models
{
    public class ResponseBase<TResult> : IResponseBase<TResult>
    {
        public ResponseBase()
        {
            Links = new Dictionary<string, ILink>();
        }

        public IDictionary<string, ILink> Links { get; private set; }

        public object Request { get; set; }

        public TResult Result { get; set; }
    }
}
