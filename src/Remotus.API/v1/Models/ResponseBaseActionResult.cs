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
    public class ResponseBaseActionResult : IHttpActionResult
    {
        private readonly ApiController _controller;

        public ResponseBaseActionResult(IResponseBase response, ApiController controller)
        {
            _controller = controller;
            Response = response;
        }

        public IResponseBase Response { get; set; }


        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            string json;
            var serializer = _controller.Configuration.Formatters.JsonFormatter.CreateJsonSerializer();
            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                jsonTextWriter.Formatting = Formatting.Indented;
                jsonTextWriter.QuoteChar = '"';

                serializer.Serialize(jsonTextWriter, Response);

                json = stringWriter.ToString();
            }

            var encoding = Encoding.UTF8;
            var mediaType = "application/json";
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.RequestMessage = _controller.Request;
            response.Content = new StringContent(json, encoding, mediaType);
            return response;
        }
    }
}
