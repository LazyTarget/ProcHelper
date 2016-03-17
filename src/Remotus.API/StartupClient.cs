using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using Owin;

namespace Remotus.API
{
    public class StartupClient
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi(app);
        }


        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Properties["InstanceID"] = Program.Service?.Client?.ClientInfo?.ClientID;
            
            var settings = new CustomJsonSerializerSettings();
            config.Formatters.JsonFormatter.SerializerSettings = settings;

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

    }
}
