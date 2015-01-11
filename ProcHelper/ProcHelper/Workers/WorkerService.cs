using System;
using System.Net;

namespace ProcHelper
{
    internal class WorkerService : IDisposable
    {
        private HttpAppHost _appHost;

        public WorkerService()
        {
            ServicePort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("ServicePort"));
        }

        private int ServicePort { get; set; }


        private void Init()
        {
            _appHost = new HttpAppHost();
            _appHost.ReceiveWebRequest += AppHost_OnReceiveWebRequest;
            _appHost.Init();
        }


        public void Start(string[] args)
        {
            Init();

            var urlBase = string.Format("http://*:{0}/", ServicePort);
            _appHost.Start(urlBase);

            this.Log().Debug("Started AppHost, url: " + urlBase);
        }


        public void Stop()
        {
            _appHost.Stop();

            this.Log().Debug("AppHost stopped");
        }

        public void Dispose()
        {
            Stop();
            _appHost.Dispose();
        }




        private void AppHost_OnReceiveWebRequest(HttpListenerContext context)
        {
            this.Log().Debug("WebRequest: [{0}]: {1}", context.Request.HttpMethod, context.Request.Url);
        }

    }
}
