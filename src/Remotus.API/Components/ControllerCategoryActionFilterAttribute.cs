using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Lux.Extensions;

namespace Remotus.API
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ControllerCategoryActionFilterAttribute : ActionFilterAttribute
    {
        public ControllerCategoryActionFilterAttribute()
        {
        }

        public ControllerCategoryActionFilterAttribute(string categoryName)
            : this()
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }


        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!string.IsNullOrEmpty(CategoryName))
            {
                var descriptor = actionContext?.ControllerContext?.ControllerDescriptor;
                if (descriptor != null && descriptor.ControllerType != null)
                {
                    var attributes = descriptor.ControllerType.GetCustomAttributes<ControllerCategoryAttribute>();
                    var match = attributes.Any(attr =>
                    {
                        var m = false;
                        if (!string.IsNullOrEmpty(attr.CategoryName))
                            m = string.Equals(CategoryName, attr.CategoryName, StringComparison.OrdinalIgnoreCase);
                        return m;
                    });

                    if (!match)
                    {
                        //actionContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
                        //actionContext.Response.Content = new StringContent($"Controller is missing category '{CategoryName}'");
                        
                        var error = new HttpError($"Controller is missing category '{CategoryName}'");

                        MediaTypeFormatter formatter = null;
                        MediaTypeHeaderValue mediaType = null;

                        // Default formatter
                        formatter = descriptor.Configuration.Formatters.XmlFormatter;
                        mediaType = formatter.SupportedMediaTypes.FirstOrDefault();

                        // Find formatter by header
                        foreach (var mediaTypeWithQualityHeaderValue in actionContext.Request.Headers.Accept)
                        {
                            formatter = descriptor.Configuration.Formatters.FindWriter(error.GetType(), mediaTypeWithQualityHeaderValue);
                            if (formatter != null)
                            {
                                mediaType = mediaTypeWithQualityHeaderValue;
                                break;
                            }
                        }

                        var actionResult = new FormattedContentResult<HttpError>(HttpStatusCode.NotFound, error, formatter, mediaType, actionContext.Request);
                        actionContext.Response = actionResult.ExecuteAsync(CancellationToken.None).WaitForResult();
                        return;
                    }
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
