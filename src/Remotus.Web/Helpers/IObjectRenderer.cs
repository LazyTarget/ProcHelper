using System.Web.Mvc;

namespace Remotus.Web.Helpers
{
    public interface IObjectRenderer
    {
        MvcHtmlString Render(object value);
    }
}