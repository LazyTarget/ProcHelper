using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Remotus.API
{
    public static class ActionResultExtensions
    {
        public static MediaTypeFormatter GetMediaTypeFormatterFromRequest<TResult>(this HttpRequestMessage request)
        {
            MediaTypeHeaderValue mediaType = null;
            var formatter = GetMediaTypeFormatterFromRequest<TResult>(request, out mediaType);
            return formatter;
        }

        public static MediaTypeFormatter GetMediaTypeFormatterFromRequest<TResult>(this HttpRequestMessage request, out MediaTypeHeaderValue mediaType)
        {
            var config = request.GetConfiguration();

            MediaTypeFormatter formatter = null;
            mediaType = null;

            // Find formatter by header
            foreach (var mediaTypeWithQualityHeaderValue in request.Headers.Accept)
            {
                var f = config.Formatters.FindWriter(typeof(TResult), mediaTypeWithQualityHeaderValue);
                if (f != null)
                {
                    formatter = f;
                    mediaType = mediaTypeWithQualityHeaderValue;
                    break;
                }
            }
            return formatter;
        }

        
        public static FormattedContentResult<TResult> CreateFormattedContentResult<TResult>(this HttpRequestMessage request, TResult result, HttpStatusCode statusCode)
        {
            MediaTypeFormatter formatter = null;
            MediaTypeHeaderValue mediaType = null;

            formatter = GetMediaTypeFormatterFromRequest<TResult>(request, out mediaType);
            if (formatter == null)
            {
                // Default formatter
                var config = request.GetConfiguration();
                formatter = config.Formatters.JsonFormatter;
                mediaType = formatter.SupportedMediaTypes.FirstOrDefault();
            }
            else if (mediaType == null)
                mediaType = formatter.SupportedMediaTypes.FirstOrDefault();

            var actionResult = new CustomFormattedContentResult<TResult>(statusCode, result, formatter, mediaType, request);
            return actionResult;
        }


        private class CustomFormattedContentResult<TRes> : FormattedContentResult<TRes>
        {
            public CustomFormattedContentResult(HttpStatusCode statusCode, TRes content, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType, HttpRequestMessage request)
                : base(statusCode, content, formatter, mediaType, request)
            {
            }

            public CustomFormattedContentResult(HttpStatusCode statusCode, TRes content, MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType, ApiController controller)
                : base(statusCode, content, formatter, mediaType, controller)
            {
            }

            public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = base.ExecuteAsync(cancellationToken);
                return response;
            }
        }

    }
}
