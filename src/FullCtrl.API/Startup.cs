using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.Owin;
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
            
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());


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
