using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Remotus.Web.Helpers
{
    public static class EditorHtmlHelperExtensions
    {
        public static MvcHtmlString ParameterEditor(this HtmlHelper htmlHelper, string name, Remotus.Base.IParameter parameter)
        {
            var result = EditorForType(htmlHelper, name, parameter.Value, parameter.Type);
            return result;
        }



        public static MvcHtmlString EditorForType(this HtmlHelper htmlHelper, string name, object value, Type type)
        {
            var result = EditorForType(htmlHelper, name, value, type, !(type?.IsValueType ?? false));
            return result;
        }
        
        private static MvcHtmlString EditorForType(this HtmlHelper htmlHelper, string name, object value, Type type, bool nullable)
        {
            MvcHtmlString result;
            if (type == null)
            {
                // differenciate between null and "" ?
                result = htmlHelper.TextBox(name, value);
            }
            else if (type.IsGenericType &&
                     type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var t = type.GetGenericArguments()[0];
                result = EditorForType(htmlHelper, name, value, t, true);
            }
            else if (type == typeof(bool))
            {
                if (nullable)
                {
                    // todo:
                }

                var isChecked = value is bool && (bool) value;
                result = htmlHelper.CheckBox(name, isChecked);
            }
            else if (type.IsEnum)
            {
                var valueAsString = (value ?? "").ToString();
                var valueIsDefined = !string.IsNullOrEmpty(valueAsString) && Enum.IsDefined(type, valueAsString);
                var parsed = valueIsDefined ? Enum.Parse(type, valueAsString) : null;
                var values = Enum.GetValues(type);

                var selectList = new List<SelectListItem>();
                if (nullable)
                {
                    selectList.Add(new SelectListItem
                    {
                        Text = "",
                        Value = "",
                        Selected = !valueIsDefined
                    });
                }

                selectList.AddRange(values.Cast<Enum>().Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString("d"),
                    Selected = parsed != null && Object.Equals(parsed, x),
                }));
                result = htmlHelper.DropDownList(name, selectList);
            }
            else
            {
                result = htmlHelper.TextBox(name, value);
            }
            return result;
        }
    }
}