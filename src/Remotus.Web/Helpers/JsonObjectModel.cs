using System;
using Lux.Model;
using Newtonsoft.Json.Linq;

namespace Remotus.Web.Helpers
{
    public class JsonObjectModel : Lux.Model.ObjectModel
    {
        public JsonObjectModel(JObject obj)
        {
            LoadFromJObject(obj);
        }

        private void LoadFromJObject(JObject obj)
        {
            ClearProperties();

            var properties = obj?.Properties();
            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var prop = DefineProperty(property.Name, null, property.Value, true);
                }
            }
        }

        public override IProperty DefineProperty(string propertyName, Type type, object value, bool isReadOnly)
        {
            return base.DefineProperty(propertyName, type, value, isReadOnly);
        }
    }
}