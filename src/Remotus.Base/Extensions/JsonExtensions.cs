using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Remotus.Base
{
    public static class JsonExtensions
    {
        public static string SerializeJson(this JsonSerializer serializer, object value)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                var stringWriter = new StringWriter(stringBuilder);
                serializer.Serialize(stringWriter, value);
                var json = stringBuilder.ToString();
                return json;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static TResult DeserializeJson<TResult>(this JsonSerializer serializer, string json)
        {
            try
            {
                var stringReader = new StringReader(json);
                var reader = new JsonTextReader(stringReader);
                var result = serializer.Deserialize<TResult>(reader);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}