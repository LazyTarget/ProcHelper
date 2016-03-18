using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
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
}
