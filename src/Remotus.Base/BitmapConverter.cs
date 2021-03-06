﻿using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace Remotus.Base
{
    public class BitmapConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Bitmap);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                reader.Skip();
                return null;
            }
            else
            {
                byte[] imgData = null;
                if (reader.TokenType == JsonToken.String)
                {
                    var base64 = (reader.Value ?? "").ToString();
                    imgData = Convert.FromBase64String(base64);
                }
                else if (reader.TokenType == JsonToken.Bytes)
                {
                    imgData = reader.ReadAsBytes();
                }

                if (imgData != null)
                {
                    try
                    {
                        using (var stream = new MemoryStream(imgData, false))
                        {
                            var img = new Bitmap(stream);
                            return img;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid image", ex);
                    }
                }
                else
                    throw new Exception($"Unexpected token or value when parsing bitmap. " +
                                        $"Token: {reader.TokenType}, Value: {reader.Value}");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
            {
                try
                {
                    if (value is Bitmap)
                    {
                        var img = value as Bitmap;
                        var format = System.Drawing.Imaging.ImageFormat.Png;
                        using (var stream = new MemoryStream())
                        {
                            img.Save(stream, format);
                            var imgData = stream.ToArray();
                            writer.WriteValue(imgData);
                        }
                    }
                    else
                        throw new Exception("Expected Bitmap object value");
                }
                catch (Exception ex)
                {
                    writer.WriteNull();
                    //throw;
                }
            }
        }
    }
}
