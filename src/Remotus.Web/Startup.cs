using Microsoft.Owin;
using Owin;
using Remotus.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace Remotus.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
