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
            // todo: check Accept header
            // todo: support for serialization to Xml?

            var toJson = true;
            if (toJson)
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
            else
            {
                throw new NotSupportedException();
            }
        }

    }
}
