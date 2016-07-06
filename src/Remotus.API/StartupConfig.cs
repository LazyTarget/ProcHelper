using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Owin;
using Remotus.API.Components;

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



            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                //map.UseCors(CorsOptions.AllowAll);

                var signalrConf = new HubConfiguration();
                signalrConf.EnableDetailedErrors = true;        // only for debug
                signalrConf.EnableJavaScriptProxies = true;     // todo: remove
                
                // You can enable JSONP by uncommenting line below.
                // JSONP requests are insecure but some older browsers (and some
                // versions of IE) require JSONP to work cross domain
                //signalrConf.EnableJSONP = true;

                var authorizer = new CustomHubAuthorizeAttribute();
                var module = new AuthorizeModule(authorizer, authorizer);
                GlobalHost.HubPipeline.AddModule(module);

                map.RunSignalR(signalrConf);
            });

            //app.MapSignalR("/signalr", signalrConf);

            _Configuration = config;
        }

    }
}
