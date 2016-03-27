using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class ResponseBaseConverter : JsonConverter
    {
        public ResponseBaseConverter()
        {
            
        }

        public override bool CanConvert(Type objectType)
        {
            var r = typeof (IResponseBase).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (IResponseBase) value;

            writer.WriteStartObject();
            writer.WritePropertyName(nameof(p.Error));
            serializer.Serialize(writer, p.Error);

            writer.WritePropertyName(nameof(p.Result));
            serializer.Serialize(writer, p.Result);

            writer.WritePropertyName(nameof(p.ResultType));
            serializer.Serialize(writer, p.ResultType);
            
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IResponseBase p = existingValue as IResponseBase ?? new DefaultResponseBase<object>();
            
            // Populate object
            JObject obj = JObject.Load(reader);
            serializer.Populate(obj.CreateReader(), p);

            try
            {
                var tmp = p.Result;
                if (tmp is JToken && p.ResultType != null && !typeof(JToken).IsAssignableFrom(p.ResultType))
                {
                    var token = (JToken)tmp;
                    var val = token.ToObject(p.ResultType, serializer);

                    var type = p.GetType();
                    var propertyInfo = type.GetProperty(nameof(p.Result));
                    if (propertyInfo != null && propertyInfo.CanWrite && propertyInfo.SetMethod != null)
                    {
                        propertyInfo.SetValue(p, val);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return p;
        }
    }
}
