using System;
using FullCtrl.Base;

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
                }
            }
        }

    }
}
