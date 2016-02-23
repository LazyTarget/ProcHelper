using System;
using FullCtrl.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FullCtrl.API
{
    class Program
    {
        private static ApiService _service;


        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                // If run via Service Control Manager
                LogManager.InitializeWith<TraceLogger>();
                RunService(args);
            }
            else
            {
                // If run via Explorer, Command prompt or other
                LogManager.InitializeWith<ConsoleLogger>();
                RunServiceWithConsole(args);
            }

        }
        
        
        private static void RunService(string[] args)
        {
            _service = new ApiService();
            var winService = new WinService(_service);
            var services = new System.ServiceProcess.ServiceBase[] { winService };
            System.ServiceProcess.ServiceBase.Run(services);
        }
        
        private static void RunServiceWithConsole(string[] args)
        {
            using (_service = new ApiService())
            {
                _service.Start(args);

                string input = null;
                while (true)
                {
                    var prevInput = input;
                    if (input == "exit")
                        break;
                    if (input == "stop")
                        Console.WriteLine("Are you sure you wish to stop the service? (y/n)");

                    input = Console.ReadLine();
                    Console.WriteLine();
                    if (input == "y" && (prevInput == "stop" || prevInput == "exit"))
                    {
                        _service.Stop();
                        break;
                    }


                    var api = new FullCtrlAPI();
                    int pid = 528;
                    Console.WriteLine("Sending request");
                    //var response = api.Process.Get(pid).WaitForResult();
                    var response = api.AudioController.GetAudioEndpoints().WaitForResult();
                    var json = Serialize(response);
                    Console.WriteLine(json);
                }
            }
        }

        private static string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };
            settings.Converters.Add(new StringEnumConverter());

            var json = JsonConvert.SerializeObject(obj, settings);
            return json;

        }

    }
}
