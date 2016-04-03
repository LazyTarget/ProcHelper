using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Remotus.Web.Rendering;

namespace Remotus.Web.Helpers
{
    public static class ObjectRendererHtmlHelperExtensions
    {
        public static MvcHtmlString RenderObject(this HtmlHelper htmlHelper, object value)
        {
            var renderer = HtmlObjectRenderer.Default;
            var result = RenderObject(htmlHelper, renderer, value);
            return result;
        }

        public static MvcHtmlString RenderObject(this HtmlHelper htmlHelper, IObjectRenderer renderer, object value)
        {
            var sb = new StringBuilder();
            try
            {
                var reference = new Lux.Model.ObjectModel();
                reference.DefineProperty("Value", null, value, true);
                reference.DefineProperty("Renderer", typeof(IObjectRenderer), renderer, true);

                using (var textWriter = new StringWriter(sb))
                {
                    renderer.Render(textWriter, reference);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
            var html = sb.ToString();
            var mvcString = MvcHtmlString.Create(html);
            return mvcString;
        }
    }
}