using System;
using System.Net;
using System.Net.Http;
using FullCtrl.Base;
using Microsoft.Owin.Hosting;

namespace FullCtrl.API
{
    internal class ApiService : IDisposable
    {
        private IDisposable _server;

        public ApiService()
        {
            
        }

        public virtual StartOptions GetStartOptions()
        {
            var options = new StartOptions();
            options.Urls.Add("http://+:9000");
            options.Urls.Add("http://localhost:9000");
            return options;
        }

        
        public void Start(string[] args)
        {
            Stop();


            Console.WriteLine("Server starting");
            
            var server = StartAsSelftHost();

            Console.WriteLine("Server started");

            _server = server;
        }


        public IDisposable StartAsSelftHost()
        {
            var options = GetStartOptions();
            var res = WebApp.Start<Startup>(options);
            return res;
        }


        public void Stop()
        {
            if (_server != null)
            {
                Console.WriteLine("Server stopping");
                _server.Dispose();
                _server = null;
                Console.WriteLine("Server stopped");
            }
        }

        public void Dispose()
        {
            Stop();
        }



        private void Server_OnReceiveWebRequest(HttpListenerContext context)
        {
            this.Log().Debug("WebRequest: [{0}]: {1}", context.Request.HttpMethod, context.Request.Url);
        }

    }
}
