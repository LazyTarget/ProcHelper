using System;
using Newtonsoft.Json;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class FunctionPluginConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var r = typeof (IFunctionPlugin).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (IFunctionPlugin) value;


            writer.WriteStartObject();

            writer.WritePropertyName(nameof(p.ID));
            serializer.Serialize(writer, p.ID);

            writer.WritePropertyName(nameof(p.Name));
            serializer.Serialize(writer, p.Name);

            writer.WritePropertyName(nameof(p.Version));
            serializer.Serialize(writer, p.Version);

            writer.WritePropertyName("Functions");
            object funcs = p.GetFunctions();
            serializer.Serialize(writer, funcs);

            writer.WriteEndObject();

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                reader.Skip();
                return null;
            }

            var val = existingValue ?? new FunctionPluginDescriptor();
            var p = (IFunctionPlugin) val;

            serializer.Populate(reader, p);

            return p;
        }
    }
}
