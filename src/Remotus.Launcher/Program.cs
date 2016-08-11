using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BrendanGrant.Helpers.FileAssociation;
using Fclp;
using Lux.IO;
using Remotus.API;
using Remotus.API.Net.Client;
using Remotus.Base;
using Remotus.Base.Scripting;

namespace Remotus.Launcher
{
    class Program
    {
        private static readonly IFileSystem _fileSystem = new FileSystem();
        private static ILog _log;
        private static System.Threading.CancellationTokenSource _cancellation;


        static void Main(string[] args)
        {
            LogManager.ConfigureLog4Net();
            LogManager.InitializeWith<Log4NetLogger>();

            _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);

            _log.Debug($"Remotus launcher :: " +
                       $"UserInteractive: {Environment.UserInteractive}. " +
                       $"Args: {String.Join(" ", args)}");

            if (!System.Diagnostics.Debugger.IsAttached)
            {
                bool attach;
                if (bool.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("AttachDebugger"), out attach) && attach)
                    System.Diagnostics.Debugger.Launch();
                else
                    System.Threading.Thread.Sleep(15 * 1000);
            }

            
            try
            {
                RunAsConsole(args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error occured in launcher");
                Environment.ExitCode = 1;
            }
            System.Diagnostics.Trace.WriteLine($"Remotus launcher, exit code: {Environment.ExitCode}");
        }
        

        private static void RunAsConsole(string[] args)
        {
            Console.WriteLine("Remotus Launcher v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("For help enter \"help\"");

            Queue<string> buffer = null;
            var firstArg = args.FirstOrDefault();
            if (firstArg == "buffer")
                buffer = new Queue<string>(args.Skip(1));
            while (true)
            {
                string input = null;
                if (buffer?.Count > 0)
                    input = buffer.Dequeue() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("> ");
                    input = Console.ReadLine();
                }
                else
                    Console.WriteLine("> " + input);

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Invalid input!");
                    continue;
                }

                if (input == "exit")
                {
                    break;
                }
                ProcessInput(input);
            }
        }
        

        private static void ProcessInput(string input)
        {
            var verb = input;
            try
            {
                if (input == "cls")
                {
                    Console.Clear();
                    return;
                }
                else if (input == "help" || input == "-?")
                {
                    PrintHelp();
                    return;
                }
                else if (input == "exit")
                {
                    return;
                }


                var args = new string[0];
                if (!string.IsNullOrWhiteSpace(input))
                    args = input.Split(' ');
                var switchIndex = args.ToList().FindIndex(x => x.StartsWith("/"));
                if (switchIndex < 0)
                    switchIndex = args.Length;
                verb = switchIndex > 0
                    ? String.Join(" ", args.Take(switchIndex))
                    : args.Length > 0 ? args[0] : null;
                args = switchIndex > 0
                    ? args.Skip(switchIndex).ToArray()
                    : args.Length > 1 ? args.Skip(1).ToArray() : args;

                var a = new CommandArguments
                {
                    Verb = verb,
                    Arguments = args,
                };
                ProcessCommand(a);
            }
            catch (AggregateException ex)
            {
                var msg = ex.GetBaseException().Message;
                Console.WriteLine($"Error executing command '{verb}': {msg}");
                System.Diagnostics.Debug.WriteLine(ex);

                if (!Environment.UserInteractive)
                    throw;
            }
            catch (Exception ex)
            {
                var msg = ex.GetBaseException().Message;
                Console.WriteLine($"Error executing command '{verb}': {msg}");
                System.Diagnostics.Debug.WriteLine(ex);

                if (!Environment.UserInteractive)
                    throw;
            }
        }


        private static void ProcessCommand(CommandArguments a)
        {
            var verb = a.Verb;
            if (verb == "cancel")
            {
                VerbCancel(a);
            }
            else if (verb == "associate")
            {
                VerbInstallFileAssociations(a);
            }
            else if (//verb == "run" ||
                     verb == "run-plugin")
            {
                VerbRunPlugin(a);
            }
            else if (verb == "run" ||
                     verb == "run-script")
            {
                VerbRunScript(a);
            }
            else
            {
                CommandNotFound(a);
            }
        }


        private static void PrintHelp()
        {
            Console.WriteLine("Commands: ");
            Console.WriteLine("* associate - Installs Remotus file associations");
            Console.WriteLine("* run-plugin - Run a Remotus plugin");
            Console.WriteLine("* run-script - Run a Remotus script file");
        }


        private static void CommandNotFound(CommandArguments a)
        {
            if (a.Arguments?.Length > 0)
                Console.WriteLine($"Command '{a.Verb}' was not found. Args: {string.Join(" ", a.Arguments)}");
            else
                Console.WriteLine($"Command '{a.Verb}' was not found.");
        }



        private static void VerbInstallFileAssociations(CommandArguments a)
        {
            object someOptionsClass = null;
            InstallFileAssociations(someOptionsClass);
        }


        private static void InstallFileAssociations(object options)
        {
            var prog = new ProgramAssociationInfo("Remotus.remotus");
            if (!prog.Exists)
            {
                prog.Create();
            }
            prog.Description = "Remotus script file";
            prog.Verbs = new ProgramVerb[]
            {
                    new ProgramVerb("Open", $"\"{Assembly.GetExecutingAssembly().Location}\" run %1"),
                    new ProgramVerb("Run", $"\"{Assembly.GetExecutingAssembly().Location}\" Run %1"),
                    //new ProgramVerb("Edit", $"\"{Assembly.GetExecutingAssembly().Location}\" \"edit %1\""),
            };


            var ext = ".remotus";
            var association = new FileAssociationInfo(ext);
            if (association.Exists)
            {
                // Exists already
            }
            else
            {
                association.Create();
                association.ContentType = "application/xml";
                association.PerceivedType = PerceivedTypes.Text;
                association.ProgID = prog.ProgID;
            }
        }




        private static void VerbCancel(CommandArguments a)
        {
            var p = new FluentCommandLineParser<CancelLastCommandArguments>();
            p.IsCaseSensitive = false;
            p.SetupHelp("help");

            var args = a.Verb == "cancel" ||
                       a.Verb == "cancel-script" ||
                       a.Verb == "cancel-plugin"
                ? a.Arguments
                : new string[0];
            var r = p.Parse(args);
            if (r.HasErrors)
            {
                Console.WriteLine("Error: " + r.ErrorText);
            }
            else if (r.HelpCalled)
            {
                Console.WriteLine($"Help for {a.Verb} not implemented...");
            }
            else
            {
                RunCancel(p.Object);
            }
        }


        private static void RunCancel(CancelLastCommandArguments arguments = null)
        {
            arguments = arguments ?? new CancelLastCommandArguments();

            if (_cancellation != null)
            {
                Console.WriteLine("Invoking cancel...");
                _cancellation.Cancel();
                Console.WriteLine("Task was cancelled...");
            }
            else
            {
                Console.WriteLine("Nothing to cancel...");
            }
        }



        private static void VerbRunScript(CommandArguments a)
        {
            var p = new FluentCommandLineParser<RunScriptArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.FilePath)
                .As('p', "path")
                .Required()
                .WithDescription("File path to plugin");
            p.Setup(x => x.Async)
                .As('a', "async")
                .WithDescription("Whether to run the command asynchronously");
            p.SetupHelp("help");

            var args = a.Verb == "run" ||
                       a.Verb == "run-script"
                ? a.Arguments
                : new string[0];
            var r = p.Parse(args);
            if (r.HasErrors)
            {
                Console.WriteLine("Error: " + r.ErrorText);
            }
            else if (r.HelpCalled)
            {
                Console.WriteLine($"Help for {a.Verb} not implemented...");
            }
            else
            {
                RunScript(p.Object);
            }
        }


        private static void RunScript(RunScriptArguments arguments = null)
        {
            arguments = arguments ?? new RunScriptArguments();
            var filePath = arguments.FilePath;
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var serializer = new XmlSerializer(typeof(Script));
            if (!_fileSystem.FileExists(filePath))
                throw new FileNotFoundException("Could not find script file", filePath);

            Script script;
            try
            {
                using (var stream = _fileSystem.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var obj = serializer.Deserialize(stream);
                    script = (Script)obj;
                }
                System.Diagnostics.Trace.WriteLine($"Script loaded: {script?.Name}/{script?.Version} [#{script?.ID}]");
            }
            catch (Exception ex)
            {
                throw;
            }

            var source = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken cancellationToken = source.Token;
            _cancellation = source;
            Task<IResponseBase> task = null;
            try
            {
                var executor = new ScriptExecutor();
                executor.Context = new ExecutionContext
                {
                    Logger = new TraceLogger(),
                    Remotus = new Remotus.API.v1.FullCtrlAPI(),
                    HubAgentFactory = new HubAgentFactory(),
                };

                var parameterCollection = new ParameterCollection();
                // todo: populate parameters

                // todo: execute via API, so that Remotus.API is the one doing the executing

                task = executor.Execute(script, parameterCollection);
                if (arguments.Async)
                    task.TryWaitAsync(cancellationToken);
                else
                    task.Wait(cancellationToken);

                var response = task.Result;
                System.Diagnostics.Trace.WriteLine($"Script executed: {response?.ResultType} | {response?.Result}");
            }
            catch (Exception ex)
            {
                
            }
            finally
            {

            }
        }



        private static void VerbRunPlugin(CommandArguments a)
        {
            var p = new FluentCommandLineParser<RunPluginArguments>();
            p.IsCaseSensitive = false;
            p.Setup(x => x.FilePath)
                .As('p', "path")
                .Required()
                .WithDescription("File path to plugin");
            p.Setup(x => x.Async)
                .As('a', "async")
                .WithDescription("Whether to run the command asynchronously");
            p.SetupHelp("help");

            var args = a.Verb == "run" ||
                       a.Verb == "run-plugin"
                ? a.Arguments
                : new string[0];
            var r = p.Parse(args);
            if (r.HasErrors)
            {
                Console.WriteLine("Error: " + r.ErrorText);
            }
            else if (r.HelpCalled)
            {
                Console.WriteLine($"Help for {a.Verb} not implemented...");
            }
            else
            {
                RunPlugin(p.Object);
            }
        }
        

        private static void RunPlugin(RunPluginArguments arguments = null)
        {
            arguments = arguments ?? new RunPluginArguments();
            var filePath = arguments.FilePath;
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var source = new System.Threading.CancellationTokenSource();
            try
            {
                _log.Info(() => $"Starting plugin @{filePath}");
                _cancellation = source;

                var pluginManager = new PluginManager(_fileSystem);
                var task = pluginManager
                    .Run(filePath)
                    .ContinueWith((t) =>
                    {
                        pluginManager?.Dispose();
                        pluginManager = null;

                        if (source == _cancellation)
                            _cancellation = null;
                    });

                AppDomain.CurrentDomain.DomainUnload += delegate (object sender, EventArgs args)
                {
                    // Closed from outside. Then invoke Dispose() on pluginManager, running Stop() on plugins and the closing the connection
                    source.Cancel(true);
                    pluginManager?.Dispose();
                };

                System.Threading.CancellationToken cancellationToken = source.Token;
                if (arguments.Async)
                    task.TryWaitAsync(cancellationToken);
                else
                    task.Wait(cancellationToken);

                _log.Info(() => $"Plugin exited @{filePath}");
            }
            catch (Exception ex)
            {
                _log.Error(() => $"Error when running plugin @{filePath}", ex);
            }
            finally
            {

            }
        }

    }
}
