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
        private string _baseAddress = "http://localhost:9000/";

        public ApiService()
        {
            
        }
        
        
        public void Start(string[] args)
        {
            Stop();


            Console.WriteLine("Server starting");
            
            var server = StartAsSelftHost();

            Console.WriteLine("Server started");



            var client = new HttpClient();
            var response = client.GetAsync(_baseAddress + "api/process").Result;

            Console.WriteLine(response);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);




            _server = server;
        }


        public IDisposable StartAsSelftHost()
        {
            return WebApp.Start<Startup>(_baseAddress);
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
