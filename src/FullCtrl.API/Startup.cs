using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(FullCtrl.API.Startup), nameof(FullCtrl.API.Startup.Configuration2))]

namespace FullCtrl.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi(app);
        }

        public void Configuration2(IAppBuilder app)
        {
            ConfigureWebApi(app);
        }


        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new ProcessToNullConverter());

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Filters.Add(new DebugActionFilter());

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{version}/{controller}/{id}",
            //    defaults: new
            //    {
            //        id = RouteParameter.Optional,
            //        version = "v1",
            //    }
            //);

            app.UseWebApi(config);
        }

        private class ProcessToNullConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                
            }

            public override bool CanConvert(Type objectType)
            {
                var res = objectType.IsAssignableFrom(typeof (System.Diagnostics.Process));
                return res;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return existingValue;
            }
        }

    }
}
