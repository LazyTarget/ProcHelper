using System;
using Newtonsoft.Json;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ServicePluginConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            var r = typeof (IServicePlugin).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (IServicePlugin) value;


            writer.WriteStartObject();

            writer.WritePropertyName(nameof(p.ID));
            serializer.Serialize(writer, p.ID);

            writer.WritePropertyName(nameof(p.Name));
            serializer.Serialize(writer, p.Name);

            writer.WritePropertyName(nameof(p.Version));
            serializer.Serialize(writer, p.Version);

            writer.WritePropertyName(nameof(p.Status));
            serializer.Serialize(writer, p.Status);

            writer.WriteEndObject();

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = existingValue ?? new ServicePluginDescriptor();
            var p = (IServicePlugin) val;

            serializer.Populate(reader, p);

            return p;
        }
    }
}
