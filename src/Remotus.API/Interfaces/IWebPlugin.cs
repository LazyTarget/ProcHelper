using System;
using System.Web.Http;
using Remotus.Base;

namespace Remotus.API
{
    [Obsolete]
    public interface IWebPlugin : IPlugin
    {
        void RegisterRoutes(HttpRouteCollection routes);
    }
}