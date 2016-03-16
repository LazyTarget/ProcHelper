using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1
{
    public class TypeInfoJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var r = typeof (Type).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (Type) value;
            var str = p.AssemblyQualifiedName;
            writer.WriteValue(str);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = (reader.Value ?? "").ToString();
            var p = Type.GetType(str);
            return p;
        }
    }
}
