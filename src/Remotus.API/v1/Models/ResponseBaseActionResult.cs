using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Remotus.Base;

namespace Remotus.API.v1.Models
{
    public class ResponseBaseActionResult : IHttpActionResult, IResponseBaseActionResult
    {
        private readonly ApiController _controller;

        public ResponseBaseActionResult(ApiController controller, IResponseBase response)
        {
            _controller = controller;
            Response = response;
        }

        public IResponseBase Response { get; set; }


        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var actionResult = _controller.Request.CreateFormattedContentResult(Response, HttpStatusCode.OK);
            var responseMessage = await actionResult.ExecuteAsync(CancellationToken.None);
            return responseMessage;
        }
    }


    public class ResponseBaseActionResult<TResult> : ResponseBaseActionResult, IResponseBaseActionResult<TResult>
    {
        public ResponseBaseActionResult(ApiController controller, IResponseBase<TResult> response)
            : base(controller, response)
        {
        }

        public new IResponseBase<TResult> Response
        {
            get { return (IResponseBase<TResult>) base.Response; }
            set { base.Response = value; }
        }
    }
}
