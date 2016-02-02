using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

//[assembly: OwinStartup(typeof(FullCtrl.API.Startup), nameof(FullCtrl.API.Startup.Configuration))]

namespace FullCtrl.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureWebApi(app);
        }


        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(config);
        }

    }
}
