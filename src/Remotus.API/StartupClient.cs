using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
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

            //config.Services.Replace(typeof(IHttpControllerSelector), new UriVersionComponentControllerSelector(config));
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{version}/{controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    version = "v1",
                    action = "Index",
                    namespaces = new string[]
                    {
                        "Remotus.API.v1.Client.Controllers",
                        "Remotus.API.v2.Client.Controllers",
                    },
                }
            );

            app.UseWebApi(config);
        }

    }
}
