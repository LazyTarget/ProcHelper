using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1
{
    public class ParameterCollectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            var r = typeof (IParameterCollection).IsAssignableFrom(objectType);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var p = (IParameterCollection) value;
            var list = p.Values.ToList();
            
            serializer.Serialize(writer, list);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var list = new List<IParameter>();
            serializer.Populate(reader, list);

            IParameterCollection val = existingValue as IParameterCollection ?? new ParameterCollection();
            list.ForEach(p => val.Add(p.Name, p));

            return val;
        }
    }
}
