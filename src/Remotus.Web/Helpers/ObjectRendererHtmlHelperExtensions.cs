using System.Web.Mvc;

namespace Remotus.Web.Helpers
{
    public static class ObjectRendererHtmlHelperExtensions
    {
        public static MvcHtmlString RenderObject(this HtmlHelper html, IObjectRenderer renderer, object value)
        {
            var result = renderer.Render(value);
            return result;
        }
    }
}