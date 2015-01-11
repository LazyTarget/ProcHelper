using System;

namespace ProcHelper
{
    class Program
    {
        private static WorkerService _service;

        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                // If run via Service Control Manager
                LogManager.InitializeWith<TraceLogger>();
                StartService(args);
            }
            else
            {
                // If run via Explorer, Command prompt or other
                LogManager.InitializeWith<ConsoleLogger>();
                StartServiceWithConsole(args);
            }
        }



        private static void StartService(string[] args)
        {
            _service = new WorkerService();
            var winService = new WinService(_service);
            var services = new System.ServiceProcess.ServiceBase[] { winService };
            System.ServiceProcess.ServiceBase.Run(services);
        }

        private static void StartServiceWithConsole(string[] args)
        {
            _service = new WorkerService();
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
                    _service.Dispose();
                    break;
                }
            }
        }
    }
}
