using System;
using System.Linq;
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
                    if (input == "exit" || input == "stop")
                    {
                        continue;
                    }
                    if (input == "y" && (prevInput == "stop" || prevInput == "exit"))
                    {
                        _service.Stop();
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
