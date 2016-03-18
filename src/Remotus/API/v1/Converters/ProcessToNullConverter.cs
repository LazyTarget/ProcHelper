using System;
using Newtonsoft.Json;

namespace Remotus.API.v1
{
    public class ProcessToNullConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            var r = typeof(System.Diagnostics.Process).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Skip();
            return existingValue;
        }
    }
}
