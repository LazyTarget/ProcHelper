using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Lux.Extensions;

namespace Remotus.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ActionCategoryActionFilterAttribute : ActionFilterAttribute
    {
        public ActionCategoryActionFilterAttribute()
        {
        }

        public ActionCategoryActionFilterAttribute(string categoryName)
            : this()
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }


        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!string.IsNullOrEmpty(CategoryName))
            {
                var descriptor = actionContext?.ActionDescriptor;
                if (descriptor != null)
                {
                    var attributes = descriptor.GetCustomAttributes<ActionCategoryAttribute>();
                    var match = attributes.Any(attr =>
                    {
                        var m = false;
                        if (!string.IsNullOrEmpty(attr.CategoryName))
                            m = string.Equals(CategoryName, attr.CategoryName, StringComparison.OrdinalIgnoreCase);
                        return m;
                    });

                    if (!match)
                    {
                        var error = new HttpError($"Action is missing category '{CategoryName}'");
                        var actionResult = actionContext.Request.CreateFormattedContentResult(error, HttpStatusCode.NotFound);
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
