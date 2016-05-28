using System.Web.Http;
using Remotus.Base;

namespace Remotus.API
{
    public interface IWebPlugin : IPlugin
    {
        void RegisterRoutes(HttpRouteCollection routes);
    }
}