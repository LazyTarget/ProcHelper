using System;
using System.Diagnostics;
using System.Net;

namespace ProcHelper
{
    class Program
    {
        private static long ServicePort = 1323;

        static void Main(string[] args)
        {
            var appHost = new HttpAppHost();
            appHost.ReceiveWebRequest += AppHost_OnReceiveWebRequest;
            appHost.Init();

            var urlBase = string.Format("http://*:{0}/", ServicePort);
            appHost.Start(urlBase);


            Console.WriteLine("AppHost started");

            
            Console.ReadLine();


            Console.WriteLine("Stopping AppHost");
            appHost.Stop();
            Console.WriteLine("AppHost stopped");

            Console.ReadLine();
        }

        private static void AppHost_OnReceiveWebRequest(HttpListenerContext context)
        {
            Trace.WriteLine(string.Format("WebRequest: [{0}]: {1}", context.Request.HttpMethod, context.Request.Url));
            Console.WriteLine(string.Format("WebRequest: [{0}]: {1}", context.Request.HttpMethod, context.Request.Url));
        }
    }
}
