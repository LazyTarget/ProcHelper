﻿using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json.Linq;
using Remotus.Base;

namespace Remotus.Web.Helpers
{
    public class DefaultObjectRenderer : IObjectRenderer
    {
        public MvcHtmlString Render(object value)
        {
            var sb = new StringBuilder();
            try
            {
                using (var textWriter = new StringWriter(sb))
                using (var writer = new HtmlTextWriter(textWriter))
                {
                    RenderObject(writer, value);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            var html = sb.ToString();
            var mvcString = MvcHtmlString.Create(html);
            return mvcString;
        }


        protected void RenderObject(HtmlTextWriter writer, object value)
        {
            var type = value?.GetType();
            var handled = CustomWriteObject(writer, value, type);
            if (handled)
            {
                return;
            }


            if (value == null)
            {
                WriteNull(writer);
            }
            else if (value is string)
            {
                var str = (string)value;

                if (!string.IsNullOrWhiteSpace(str) && Remotus.Base.Extensions.IsBase64String(str))
                {
                    var base64 = str.Trim();
                    WriteBase64(writer, base64);
                }
                else
                {
                    WriteString(writer, str);
                }
            }
            else if (value is JToken)
            {
                var jToken = (JToken)value;
                WriteJToken(writer, jToken);
            }
            else if (value is Bitmap)
            {
                var bitmap = (Bitmap)value;
                WriteImage(writer, bitmap);
            }
            else if (value is IDictionary)
            {
                var dictionary = (IDictionary)value;
                WriteDictionary(writer, dictionary);
            }
            else if (value is IList)
            {
                var list = (IList)value;
                WriteList(writer, list);
            }
            else if (value is IEnumerable)
            {
                var enumerable = (IEnumerable)value;
                WriteEnumerable(writer, enumerable);
            }
            else
            {
                WriteObject(writer, value);
            }
        }


        /// <summary>
        /// Returns whether the object has been handled
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual bool CustomWriteObject(HtmlTextWriter writer, object value, Type type)
        {
            if (value is Type)
            {
                var t = (Type)value;
                var typeString = t.FullName + ", " + t.Assembly.GetName().Name;

                using (writer.BeginTag("span"))
                {
                    using (writer.BeginTagAttributes())
                    {
                        writer.WriteAttribute("class", "Value String TypeString");
                    }

                    writer.Write(typeString);
                    return true;
                }
            }
            else if ((type != null && type.IsPrimitive) ||
                     value is TimeSpan ||
                     value is Enum ||
                     value is DateTime)
            {
                var str = value.ToString();

                var t = value.GetType();
                var typeString = t.FullName + ", " + t.Assembly.GetName().Name;

                WriteString(writer, str);
                return true;

                //if (!string.IsNullOrWhiteSpace(str))
                //{
                //    //<span class="String" data-type="@(typeString)">
                //    //    @str
                //    //</span>
                //}
                //else if (str == null)
                //{
                //    //<span class="String Empty NullValue" data-type="@(typeString)">
                //    //    [NULL]
                //    //</span>
                //}
                //else
                //{
                //    //<span class="String Empty" data-type="@(typeString)">
                //    //    @MvcHtmlString.Create("&nbsp;")
                //    //</span>
                //}
            }

            return false;
        }


        #region Generic

        protected virtual void WriteNull(HtmlTextWriter writer)
        {
            using (writer.BeginTag("span"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Value NullValue");
                }

                var str = "[NULL]";
                writer.Write(str);
            }
        }

        protected virtual void WriteEmpty(HtmlTextWriter writer)
        {
            using (writer.BeginTag("span"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Value Empty");
                }

                var value = "&nbsp;";
                writer.Write(value);
            }
        }
        
        protected virtual void WriteString(HtmlTextWriter writer, string value)
        {
            if (value == null)
            {
                WriteNull(writer);
            }
            else if (string.IsNullOrWhiteSpace(value))
            {
                WriteEmpty(writer);
            }
            else
            {
                using (writer.BeginTag("span"))
                {
                    using (writer.BeginTagAttributes())
                    {
                        writer.WriteAttribute("class", "Value String");
                    }

                    writer.Write(value);
                }
            }
        }

        protected virtual void WriteBase64(HtmlTextWriter writer, string base64)
        {
            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Image Base64");
                }

                var isImage = true;
                if (isImage)
                {
                    using (writer.BeginTag("img"))
                    {
                        using (writer.BeginTagAttributes())
                        {
                            var data = "data:image/png;base64," + base64;
                            writer.WriteAttribute("src", data);

                            writer.WriteAttribute("alt", $"img #{string.Join("", base64.Skip(base64.Length - 30))}");
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        protected virtual void WriteImage(HtmlTextWriter writer, Bitmap bitmap)
        {
            var format = System.Drawing.Imaging.ImageFormat.Png;
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, format);
                var imgData = stream.ToArray();
                var base64 = Convert.ToBase64String(imgData);

                WriteBase64(writer, base64);
            }
        }

        protected virtual void WriteObject(HtmlTextWriter writer, object value)
        {
            if (value == null)
            {
                WriteNull(writer);
            }
            else
            {
                using (writer.BeginTag("div"))
                {
                    using (writer.BeginTagAttributes())
                    {
                        writer.WriteAttribute("class", "Object ObjectMirror");
                    }

                    var count = 0;
                    var objectMirror = new Lux.Model.MirrorObjectModel(value);
                    var properties = objectMirror.GetProperties();
                    foreach (var property in properties)
                    {
                        //<div class="Property ObjectMirrorProperty">
                        //    <span class="PropertyName">@property.Name</span><br />
                        //    <div class="PropertyValue ObjectMirrorPropertyValue">
                        //        @RenderObject(property.Value)
                        //    </div>
                        //</div>

                        this.Log().Info($"Object: {value.GetType()}: Property: {property.Name}");

                        count++;
                        using (writer.BeginTag("div"))
                        {
                            using (writer.BeginTagAttributes())
                            {
                                writer.WriteAttribute("class", "Property ObjectMirrorProperty");
                            }

                            using (writer.BeginTag("span"))
                            {
                                using (writer.BeginTagAttributes())
                                {
                                    writer.WriteAttribute("class", "PropertyName");
                                }
                                WriteString(writer, property.Name);
                            }

                            using (writer.BeginTag("div"))
                            {
                                using (writer.BeginTagAttributes())
                                {
                                    writer.WriteAttribute("class", "PropertyValue ObjectMirrorPropertyValue");
                                }
                                RenderObject(writer, property.Value);
                            }
                        }
                    }

                    if (count <= 0)
                    {
                        WriteEmpty(writer);
                    }
                }
            }
        }

        #endregion


        #region Collections

        protected virtual void WriteEnumerable(HtmlTextWriter writer, IEnumerable enumerable)
        {
            var count = 0;

            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Enumerable");
                }

                foreach (var x in enumerable)
                {
                    //<div class="EnumerableItem">
                    //    @RenderObject(x)
                    //</div>

                    count++;
                    using (writer.BeginTag("div"))
                    {
                        using (writer.BeginTagAttributes())
                        {
                            writer.WriteAttribute("class", "EnumerableItem");
                        }

                        RenderObject(writer, x);
                    }
                }

                if (count <= 0)
                {
                    WriteEmpty(writer);
                }
            }
        }

        protected virtual void WriteList(HtmlTextWriter writer, IList list)
        {
            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Enumerable List");
                }

                if (list.Count > 0)
                {
                    foreach (var x in list)
                    {
                        //<div class="EnumerableItem ListItem">
                        //    @RenderObject(x)
                        //</div>

                        using (writer.BeginTag("div"))
                        {
                            using (writer.BeginTagAttributes())
                            {
                                writer.WriteAttribute("class", "EnumerableItem ListItem");
                            }

                            RenderObject(writer, x);
                        }
                    }
                }
                else
                {
                    WriteEmpty(writer);
                }
            }
        }

        protected virtual void WriteDictionary(HtmlTextWriter writer, IDictionary dictionary)
        {
            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Enumerable Dictionary");
                }

                if (dictionary.Count > 0)
                {
                    for (var i = 0; i < dictionary.Count; i++)
                    {
                        //<div class="EnumerableItem KeyValuePair">
                        //    <div class="KeyValuePairKey">
                        //        @RenderObject(x)
                        //    </div>
                        //    <div class="KeyValuePairValue">
                        //        @RenderObject(x)
                        //    </div>
                        //</div>

                        var key = dictionary.Keys.Cast<object>().ElementAt(i);
                        var val = dictionary.Values.Cast<object>().ElementAt(i);

                        using (writer.BeginTag("div"))
                        {
                            using (writer.BeginTagAttributes())
                            {
                                writer.WriteAttribute("class", "EnumerableItem KeyValuePair");
                            }


                            using (writer.BeginTag("div"))
                            {
                                using (writer.BeginTagAttributes())
                                {
                                    writer.WriteAttribute("class", "KeyValuePairKey");
                                }
                                RenderObject(writer, key);
                            }

                            using (writer.BeginTag("div"))
                            {
                                using (writer.BeginTagAttributes())
                                {
                                    writer.WriteAttribute("class", "KeyValuePairValue");
                                }
                                RenderObject(writer, val);
                            }
                        }
                    }
                }
                else
                {
                    WriteEmpty(writer);
                }
            }
        }

        #endregion


        #region Json

        protected virtual void WriteJToken(HtmlTextWriter writer, JToken jToken)
        {
            if (jToken is JObject)
            {
                var jObject = (JObject)jToken;
                WriteJObject(writer, jObject);
            }
            else if (jToken is JArray)
            {
                var jArray = (JArray)jToken;
                WriteJArray(writer, jArray);
            }
            else if (jToken is JValue)
            {
                var jValue = (JValue)jToken;
                WriteJValue(writer, jValue);
            }
            else
            {
                var str = jToken?.ToString();

                //WriteString(writer, str);
                RenderObject(writer, str);
            }
        }

        protected virtual void WriteJValue(HtmlTextWriter writer, JValue jValue)
        {
            //<div class="JValue">
            //    @RenderObject(jValue.Value)
            //</div>
            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "JValue");
                }
                RenderObject(writer, jValue.Value);
            }
        }

        protected virtual void WriteJObject(HtmlTextWriter writer, JObject jObject)
        {
            var count = 0;

            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Object JObject");
                }

                foreach (var property in jObject.Properties())
                {
                    //<div class="Property JObjectProperty">
                    //    <span class="PropertyName">@property.Name</span><br />
                    //    <div class="PropertyValue JObjectPropertyValue">
                    //        @RenderObject(property.Value)
                    //    </div>
                    //</div>

                    count++;
                    using (writer.BeginTag("div"))
                    {
                        using (writer.BeginTagAttributes())
                        {
                            writer.WriteAttribute("class", "Property JObjectProperty");
                        }

                        using (writer.BeginTag("span"))
                        {
                            using (writer.BeginTagAttributes())
                            {
                                writer.WriteAttribute("class", "PropertyName");
                            }
                            WriteString(writer, property.Name);
                        }

                        using (writer.BeginTag("div"))
                        {
                            using (writer.BeginTagAttributes())
                            {
                                writer.WriteAttribute("class", "PropertyValue JObjectPropertyValue");
                            }
                            RenderObject(writer, property.Value);
                        }
                    }
                }

                if (count <= 0)
                {
                    WriteEmpty(writer);
                }
            }
        }

        protected virtual void WriteJArray(HtmlTextWriter writer, JArray jArray)
        {
            using (writer.BeginTag("div"))
            {
                using (writer.BeginTagAttributes())
                {
                    writer.WriteAttribute("class", "Enumerable JArray");
                }

                if (jArray.Count > 0)
                {
                    foreach (var x in jArray)
                    {
                        //<div class="EnumerableItem JToken">
                        //    @RenderObject(x)
                        //</div>

                        using (writer.BeginTag("div"))
                        {
                            using (writer.BeginTagAttributes())
                            {
                                writer.WriteAttribute("class", "EnumerableItem JToken");
                            }

                            RenderObject(writer, x);
                        }
                    }
                }
                else
                {
                    WriteEmpty(writer);
                }
            }
        }

        #endregion

    }
}