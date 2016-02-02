using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace FullCtrl.API
{
    class Program
    {
        private static ManualResetEvent _stopEvent = new ManualResetEvent(false);
        private static string _baseAddress = "http://localhost:9000/";
        private static IDisposable _server;
        private static bool _shouldExit;


        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Start(args);

                while (!_shouldExit)
                {
                    var input = Console.ReadLine();
                    if (input == "exit")
                    {
                        _shouldExit = true;
                        _stopEvent.Set();
                    }
                }
            }
            else
            {
                Start(args);

                while (!_shouldExit)
                {
                    _stopEvent.WaitOne();
                }
            }

            // Stop
            Stop();

        }


        public static void Start(string[] args)
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

        public static IDisposable StartAsSelftHost()
        {
            return WebApp.Start<Startup>(_baseAddress);
        }

        public static void Stop()
        {
            if (_server != null)
            {
                Console.WriteLine("Server stopping");
                _server.Dispose();
                _server = null;
                Console.WriteLine("Server stopped");
            }
        }

    }
}
