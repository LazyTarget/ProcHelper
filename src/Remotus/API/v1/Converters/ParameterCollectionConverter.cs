﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Remotus.Base;

namespace Remotus.API.v1
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
            if (reader.TokenType == JsonToken.Null)
            {
                reader.Skip();
                return null;
            }

            var list = new List<IParameter>();
            serializer.Populate(reader, list);

            IParameterCollection val = existingValue as IParameterCollection ?? new ParameterCollection();
            list.ForEach(p => val.Add(p.Name, p));

            return val;
        }
    }
}
