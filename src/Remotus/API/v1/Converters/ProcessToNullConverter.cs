using System;
using Newtonsoft.Json;

namespace Remotus.API.v1
{
    public class ProcessToNullConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override bool CanConvert(Type objectType)
        {
            var res = objectType.IsAssignableFrom(typeof(System.Diagnostics.Process));
            return res;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }
    }
}
