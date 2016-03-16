using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1
{
    public class FunctionDescriptorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var r = typeof (IFunctionDescriptor).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (IFunctionDescriptor) value;


            writer.WriteStartObject();
            writer.WritePropertyName(nameof(p.Name));
            serializer.Serialize(writer, p.Name);

            writer.WritePropertyName("Parameters");
            object parameters = p.GetParameters();
            serializer.Serialize(writer, parameters);

            writer.WriteEndObject();

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var val = existingValue ?? new FunctionDescriptor();
            var p = (IFunctionDescriptor) val;

            serializer.Populate(reader, p);

            return p;
        }
    }
}
