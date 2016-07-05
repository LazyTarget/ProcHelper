using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Microsoft.AspNet.SignalR;
using Owin;

namespace Remotus.API
{
    public class StartupConfig
    {
        public HttpConfiguration _Configuration;

        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi(app);
        }


        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.Properties["InstanceID"] = Program.Service?.ClientInfo?.ClientID;

            // Formatters
            var settings = new CustomJsonSerializerSettings();
            config.Formatters.JsonFormatter.SerializerSettings = settings;

            // Services
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            // Filters
            config.Filters.Add(new DebugActionFilter());
            //config.Filters.Add(new ControllerCategoryActionFilterAttribute("Server"));
            ////config.Filters.Add(new ActionCategoryActionFilterAttribute("Server"));
            //config.Filters.Add(new ControllerCategoryActionFilterAttribute("Client"));
            ////config.Filters.Add(new ActionCategoryActionFilterAttribute("Client"));

            // Routes
            config.MapHttpAttributeRoutes();

            //var r = config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{version}/{controller}/{action}/{id}",
            //    defaults: new
            //    {
            //        id = RouteParameter.Optional,
            //        version = "v1",
            //        action = "Index",
            //    }
            //);
            //r.DataTokens["Namespaces"] = new string[]
            //{
            //    "Remotus.API.v1.Client.Controllers",
            //    "Remotus.API.v2.Client.Controllers"
            //};

            app.UseWebApi(config);



            var signalrConf = new HubConfiguration();
            signalrConf.EnableDetailedErrors = true;        // only for debug
            signalrConf.EnableJavaScriptProxies = true;     // todo: remove
            signalrConf.EnableJSONP = true;                 // todo: remove
            app.MapSignalR("/signalr", signalrConf);

            _Configuration = config;
        }

    }
}
