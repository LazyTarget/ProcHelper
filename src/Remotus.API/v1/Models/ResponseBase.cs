using System;
using System.Collections.Generic;
using System.Net.Http;
using Remotus.Base;

namespace Remotus.API.v1.Models
{
    public static class ResponseBase
    {
        public static ResponseBase<TResult> Create<TResult>(HttpRequestMessage request)
        {
            var serverRootUri = new Uri(request.RequestUri.GetLeftPart(UriPartial.Authority));

            var response = new ResponseBase<TResult>();
            response.Links["self"] = DefaultLink.FromUri(request.RequestUri);
            response.Links["root"] = DefaultLink.FromUri(new Uri(serverRootUri, "api/v1"));
            return response;
        }

        public static ResponseBase<TResult> Create<TResult>(HttpRequestMessage request, TResult result)
        {
            var response = Create<TResult>(request);
            response.Result = result;
            return response;
        }

        public static ResponseBase<TResult> Create<TResult>(HttpRequestMessage request, TResult result, IError error)
        {
            var response = Create<TResult>(request);
            response.Result = result;
            response.Error = error;
            return response;
        }

        public static ResponseBase<TResult> CreateError<TResult>(HttpRequestMessage request, IError error)
        {
            var response = Create<TResult>(request);
            response.Error = error;
            return response;
        }

        public static ResponseBase<object> Create(HttpRequestMessage request, object result)
        {
            var response = Create<object>(request);
            response.Result = result;
            return response;
        }

        public static ResponseBase<object> CreateError(HttpRequestMessage request, IError error)
        {
            var response = Create<object>(request);
            response.Error = error;
            return response;
        }
    }


    public class ResponseBase<TResult> : IResponseBase<TResult>, IResponseMetadata
    {
        private Type _type;

        public ResponseBase()
        {
            Links = new Dictionary<string, ILink>();
        }

        public IDictionary<string, ILink> Links { get; set; }

        public IError Error { get; set; }

        public TResult Result { get; set; }

        object IResponseBase.Result
        {
            get { return Result; }
        }

        public Type ResultType
        {
            get { return _type ?? typeof(TResult) ?? Result?.GetType(); }
            set { _type = value; }
        }
    }
}
