using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Remotus
{
    public class IoCJsonConverter : JsonConverter
    {
        private IContainer _container;

        public IoCJsonConverter(IContainer container)
        {
            Container = container;
        }

        public IContainer Container
        {
            get { return _container; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _container = value;
            }
        }


        public override bool CanConvert(Type objectType)
        {
            var res = _container.IsBound(objectType);
            return res;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                reader.Skip();
                return null;
            }

            object result;
            var target = serializer.Deserialize<JToken>(reader);
            if (target.Type != JTokenType.Null)
            {
                result = _container.Resolve(objectType);
                serializer.Populate(target.CreateReader(), result);
            }
            else
                result = null;
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}