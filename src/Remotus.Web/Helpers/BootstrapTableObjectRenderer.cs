using System.Linq;
using System.Web.UI;

namespace Remotus.Web.Helpers
{
    public class BootstrapTableObjectRenderer : DefaultObjectRenderer
    {
        protected override void WriteObjectModel(HtmlTextWriter writer, Lux.Model.IObjectModel value)
        {
            if (value == null)
            {
                WriteNull(writer);
                return;
            }

            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "table-responsive");
                }

                using (writer.BeginTag("table"))
                {
                    using (writer.BeginTagAttributes())
                    {
                        writer.WriteAttribute("class", "table table-striped table-hover");
                    }

                    
                    var properties = value.GetProperties().ToList();

                    // Header
                    using (writer.BeginFullTag("thead"))
                    using (writer.BeginFullTag("tr"))
                    {
                        foreach (var property in properties)
                        {
                            using (writer.BeginFullTag("th"))
                            {
                                //writer.Write(property.Name);
                                WriteString(writer, property.Name);
                            }
                        }
                    }

                    // Body
                    using (writer.BeginFullTag("tr"))
                    {
                        foreach (var property in properties)
                        {
                            using (writer.BeginFullTag("td"))
                            {
                                RenderObject(writer, property.Value);
                            }
                        }
                    }
                }
            }

        }
    }
}