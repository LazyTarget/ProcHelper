using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using Lux.Extensions;
using Newtonsoft.Json;
using Remotus.Base;

namespace Remotus.API
{
    class Program
    {
        internal static ServiceInstance Service;


        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_OnFirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_OnUnhandledException;
            
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

        private static void CurrentDomain_OnFirstChanceException(object sender, FirstChanceExceptionEventArgs eventArgs)
        {
            if (eventArgs.Exception.Source == "AudioSwitcher.AudioApi.CoreAudio")
                return;

            LogManager.GetLoggerFor("OnFirstChanceException").Error(() => "FirstChanceException", eventArgs.Exception);
        }

        private static void CurrentDomain_OnUnhandledException(object sender, UnhandledExceptionEventArgs eventArgs)
        {
            LogManager.GetLoggerFor("OnUnhandledException").Fatal(() => "UnhandledException", eventArgs.ExceptionObject as Exception);
        }


        private static void RunService(string[] args)
        {
            Service = new ServiceInstance();
            var winService = new WinService.WinService(Service);
            var services = new System.ServiceProcess.ServiceBase[] { winService };
            System.ServiceProcess.ServiceBase.Run(services);
        }
        
        private static void RunServiceWithConsole(string[] args)
        {
            using (Service = new ServiceInstance())
            {
                Service.Start(args);

                string input = null;
                while (true)
                {
                    var prevInput = input;
                    if (input == "exit")
                        break;
                    if (input == "stop")
                        Console.WriteLine("Are you sure you wish to stop the service? (y/n)");

                    input = Console.ReadLine();
                    if (input == "cls")
                    {
                        Console.Clear();
                        continue;
                    }
                    if (input == "exit" || input == "stop")
                    {
                        continue;
                    }
                    if (input == "y" && (prevInput == "stop" || prevInput == "exit"))
                    {
                        Service.Stop();
                        break;
                    }


                    string json;
                    var api = new FullCtrlAPI();

                    //int pid = 528;
                    //Console.WriteLine("Sending request");
                    //var response = api.Process.Get(pid).WaitForResult();
                    //json = Serialize(response);
                    //Console.WriteLine(json);

                    Console.WriteLine("Sending request");
                    var response4 = api.Process.List().WaitForResult();
                    var l = response4.Result.Where(
                        x => !x.HasExited && !string.IsNullOrEmpty(x.MainWindowTitle) && x.MainWindowHandle > 0)
                        .ToList();
                    json = Serialize(l);
                    Console.WriteLine(json);

                    int pid = 0;
                    Console.WriteLine("");
                    Console.Write("Enter Pid: ");
                    input = Console.ReadLine();
                    int.TryParse(input, out pid);

                    var p = api.Process.SwitchToMainWindow(pid).WaitForResult();

                    //Console.WriteLine("Sending request");
                    //var response = api.AudioController.GetAudioDevices().WaitForResult();
                    //json = Serialize(response);
                    //Console.WriteLine(json);

                    //Console.WriteLine("Sending request");
                    //var response2 = api.AudioController.GetAudioSessions().WaitForResult();
                    //json = Serialize(response2);
                    //Console.WriteLine(json);

                    //var deviceID = response?.Result?.FirstOrDefault()?.ID;
                    //if (deviceID != null)
                    //{
                    //    Console.WriteLine("Sending request");
                    //    var response3 = api.AudioController.SetDefaultDevice(deviceID).WaitForResult();
                    //    json = Serialize(response3);
                    //    Console.WriteLine(json);
                    //}
                }
            }
        }

        private static string Serialize(object obj)
        {
            JsonSerializerSettings settings;
            //settings = new JsonSerializerSettings
            //{
            //    Formatting = Formatting.Indented,
            //};
            //settings.Converters.Add(new StringEnumConverter());
            settings = new CustomJsonSerializerSettings();

            var json = JsonConvert.SerializeObject(obj, settings);
            return json;

        }

    }
}
