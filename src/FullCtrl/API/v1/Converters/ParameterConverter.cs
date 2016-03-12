using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullCtrl.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FullCtrl.API.v1
{
    public class ParameterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var r = typeof (IParameter).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (IParameter) value;
            
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(p.Name));
            serializer.Serialize(writer, p.Name);

            writer.WritePropertyName(nameof(p.Required));
            serializer.Serialize(writer, p.Required);

            writer.WritePropertyName(nameof(p.Value));
            serializer.Serialize(writer, p.Value);

            writer.WritePropertyName(nameof(p.Type));
            serializer.Serialize(writer, p.Type);
            //writer.WriteValue(p.Type?.AssemblyQualifiedName);
            
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IParameter p = existingValue as IParameter ?? new Parameter();
            
            // Populate object
            JObject obj = JObject.Load(reader);
            serializer.Populate(obj.CreateReader(), p);

            // Update Value using a converter if a Type is specified
            var prop = obj.Property(nameof(p.Value));
            if (prop?.Value.ToObject<JValue>().Value != null && p.Type != null)
            {
                var converter = new Lux.Converter();
                var val = prop.Value.ToObject<JValue>().Value;
                val = converter.Convert(val, p.Type);
                p.Value = val;
            }
            return p;
        }
    }
}
