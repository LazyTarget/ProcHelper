using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remotus.Web.Rendering
{
    public class JsonObjectRenderer : IObjectRenderer
    {
        private JsonSerializer _jsonSerializer = JsonSerializer.Create();
        private Formatting _formatting = Formatting.Indented;

        public JsonObjectRenderer()
        {
            
        }

        public virtual bool CanRender(object value)
        {
            return true;
        }

        public virtual void Render(TextWriter textWriter, object value)
        {
            RenderObject(textWriter, value);
        }

        protected virtual void RenderObject(TextWriter writer, object value)
        {
            if (value is Lux.Model.IObjectModel)
            {
                var jobj = new JObject();
                var obj = (Lux.Model.IObjectModel) value;
                var properties = obj.GetProperties();
                foreach (var prop in properties)
                {
                    var p = new JProperty(prop.Name);
                    p.Value = JToken.FromObject(prop.Value, _jsonSerializer);
                    jobj.Add(p);
                }

                RenderObject(writer, jobj);
            }
            else if (value is JToken)
            {
                var token = (JToken) value;
                var str = token.ToString(_formatting);
                writer.Write(str);
            }
            else
            {
                var token = JToken.FromObject(value, _jsonSerializer);
                var str = token.ToString(_formatting);
                writer.Write(str);
            }
        }
    }
}