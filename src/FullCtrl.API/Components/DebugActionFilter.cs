using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FullCtrl.API
{
    public class DebugActionFilter : ActionFilterAttribute
    {
        public DebugActionFilter()
        {
            
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}