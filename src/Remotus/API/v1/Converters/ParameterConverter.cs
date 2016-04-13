using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ParameterConverter : JsonConverter
    {
        public ParameterConverter()
        {
            
        }

        public override bool CanWrite => false;

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
            if (reader.TokenType == JsonToken.Null)
            {
                reader.Skip();
                return null;
            }

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
