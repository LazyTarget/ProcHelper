using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using FullCtrl.Base;
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

            
            var container = new UnityAdaptorContainer();
            container.Bind(typeof(IResponseBase), typeof(DefaultResponseBase<>));
            container.Bind(typeof(IResponseBase<>), typeof(DefaultResponseBase<>));
            container.Bind(typeof(IError), typeof(DefaultError));
            container.Bind(typeof(ILink), typeof(DefaultLink));
            container.Bind(typeof(IProcessDto), typeof(ProcessDto));
            container.Bind(typeof(IParameter), typeof(Base.Parameter));
            container.Bind(typeof(IParameterCollection), typeof(ParameterCollection));
            container.Bind(typeof(IFunctionResult), typeof(FunctionResult));
            container.Bind(typeof(IFunctionArguments), typeof(FunctionArguments));
            container.Bind(typeof(IFunctionDescriptor), typeof(API.v1.FunctionDescriptor));
            container.Bind(typeof(IPlugin), typeof(API.v1.FunctionPluginDescriptor));
            container.Bind(typeof(IFunctionPlugin), typeof(API.v1.FunctionPluginDescriptor));

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new API.v1.TypeInfoJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new API.v1.ParameterConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new API.v1.ParameterCollectionConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new API.v1.FunctionDescriptorConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new API.v1.FunctionPluginConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new ProcessToNullConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IoCJsonConverter(container));
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new BitmapConverter());

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
