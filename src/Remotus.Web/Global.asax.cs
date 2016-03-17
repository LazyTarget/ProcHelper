using System;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Remotus.Base;

namespace Remotus.Web
{
    public class MvcApplication : HttpApplication
    {
        static MvcApplication()
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_OnFirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_OnUnhandledException;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        private static void CurrentDomain_OnFirstChanceException(object sender, FirstChanceExceptionEventArgs eventArgs)
        {
            LogManager.GetLoggerFor("OnFirstChanceException").Error(() => "FirstChanceException", eventArgs.Exception);
        }

        private static void CurrentDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            LogManager.GetLoggerFor("OnUnhandledException").Fatal(() => "UnhandledException", eventArgs.ExceptionObject as Exception);
        }

    }
}
