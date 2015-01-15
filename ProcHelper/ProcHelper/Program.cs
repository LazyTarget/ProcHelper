using System;
using System.IO;
using System.Linq;

namespace ProcHelper
{
    class Program
    {
        internal static bool _envChanged;
        private static WorkerService _service;

        static void Main(string[] args)
        {
            typeof(Program).Log().Log().Debug("Starting ProcHelper");

            // Load config
            ApplySearchFolders();


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


        private static void ApplySearchFolders()
        {
            var t = System.Configuration.ConfigurationManager.AppSettings.Get("SearchPaths");
            if (!string.IsNullOrEmpty(t))
            {
                var paths = t.Split(';').ToList();
                for (var i = 0; i < paths.Count; i++)
                {
                    var x = paths[i];
                    if (string.IsNullOrWhiteSpace(x) || !Directory.Exists(x))
                        paths.RemoveAt(i--);
                }

                var val = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) ?? "";
                var newVal = string.Join(";", paths.ToArray());
                val = string.Format("{0};{1};", newVal, val.Trim(';'));
                Environment.SetEnvironmentVariable("Path", val, EnvironmentVariableTarget.Process);
                _envChanged = true;
            }


            t = System.Configuration.ConfigurationManager.AppSettings.Get("SearchFileExt");
            if (!string.IsNullOrEmpty(t))
            {
                var exts = t.Split(';').ToList();
                for (var i = 0; i < exts.Count; i++)
                {
                    var x = exts[i];
                    if (string.IsNullOrWhiteSpace(x))
                        exts.RemoveAt(i--);
                }

                var val = Environment.GetEnvironmentVariable("PathExt", EnvironmentVariableTarget.Machine) ?? "";
                var newVal = string.Join(";", exts.ToArray());
                val = string.Format("{0};{1};", newVal, val.Trim(';'));
                Environment.SetEnvironmentVariable("PathExt", val, EnvironmentVariableTarget.Process);
                _envChanged = true;
            }

            typeof (Program).Log().Log().Debug("Path: " + Environment.GetEnvironmentVariable("Path"));
            typeof (Program).Log().Log().Debug("PathExt: " + Environment.GetEnvironmentVariable("PathExt"));
        }
    }
}
