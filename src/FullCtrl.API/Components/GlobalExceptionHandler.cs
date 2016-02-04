using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using FullCtrl.API.Interfaces;
using FullCtrl.API.Models;
using FullCtrl.API.v1.Models;
using FullCtrl.Base;

namespace FullCtrl.API
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var error = Error.FromException(context.Exception);
            var response = ResponseBase.CreateError(context.Request, error);

            MediaTypeFormatter formatter = context.RequestContext.Configuration.Formatters.JsonFormatter;
            var msg = new HttpResponseMessage();
            msg.StatusCode = HttpStatusCode.InternalServerError;
            msg.Content = new ObjectContent<IResponseBase<object>>(response, formatter);

            context.Result = new ResponseMessageResult(msg);

        }
    }
}