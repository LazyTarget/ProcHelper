using System;
using System.Collections.Generic;

namespace Remotus.Base
{
    public class DefaultResponseBase
    {
        public static DefaultResponseBase<TResult> Create<TResult>()
        {
            var response = new DefaultResponseBase<TResult>();
            return response;
        }

        public static DefaultResponseBase<TResult> Create<TResult>(TResult result)
        {
            var response = Create<TResult>();
            response.Result = result;
            return response;
        }

        public static DefaultResponseBase<TResult> Create<TResult>(TResult result, IError error)
        {
            var response = Create<TResult>();
            response.Result = result;
            response.Error = error;
            return response;
        }

        public static DefaultResponseBase<TResult> CreateError<TResult>(IError error)
        {
            var response = Create<TResult>();
            response.Error = error;
            return response;
        }

        public static DefaultResponseBase<object> Create(object result)
        {
            var response = Create<object>();
            response.Result = result;
            return response;
        }

        public static DefaultResponseBase<object> CreateError(IError error)
        {
            var response = Create<object>();
            response.Error = error;
            return response;
        }
    }

    public class DefaultResponseBase<TResult> : IResponseBase<TResult>, IResponseMetadata
    {
        private Type _type;

        public DefaultResponseBase()
        {
            Links = new Dictionary<string, ILink>();
        }

        public IDictionary<string, ILink> Links { get; set; }
        public IError Error { get; set; }
        public TResult Result { get; set; }

        public Type ResultType
        {
            get { return _type ?? typeof(TResult) ?? Result?.GetType(); }
            set { _type = value; }
        }

        object IResponseBase.Result
        {
            get { return Result; }
        }


        public static DefaultResponseBase<TResult> Create(TResult result = default(TResult))
        {
            var response = new DefaultResponseBase<TResult>();
            response.Result = result;
            return response;
        }
    }
}
