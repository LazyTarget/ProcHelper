using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;
using BrendanGrant.Helpers.FileAssociation;
using Lux.Extensions;
using Lux.IO;
using Remotus.API;
using Remotus.API.Hubs.Client;
using Remotus.Base;
using Remotus.Base.Scripting;
using ExecutionContext = Remotus.API.ExecutionContext;

namespace Remotus.Launcher
{
    class Program
    {
        private static readonly IFileSystem _fileSystem = new FileSystem();
        private static readonly ILog _log = LogManager.GetLoggerFor(MethodBase.GetCurrentMethod()?.DeclaringType?.FullName);


        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                LogManager.InitializeWith<ConsoleLogger>();
            }
            else
            {
                LogManager.InitializeWith<TraceLogger>();
            }


            System.Diagnostics.Trace.WriteLine($"Remotus launcher!!! " +
                                               $"UserInteractive: {Environment.UserInteractive}. " +
                                               $"Command line: {Environment.CommandLine} " +
                                               $"Args: {String.Join(" ", args)}");
            
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                bool attach;
                if (bool.TryParse(System.Configuration.ConfigurationManager.AppSettings.Get("AttachDebugger"), out attach) && attach)
                    System.Diagnostics.Debugger.Launch();
            }

            try
            {
                var action = args?.Length > 0 ? args[0] : null;
                if (string.IsNullOrWhiteSpace(action))
                {

                }
                else if (string.Equals(action, "plugin", StringComparison.InvariantCultureIgnoreCase))
                {
                    System.Threading.Thread.Sleep(15 * 1000);
                    var path = args[1];
                    StartPlugin(path);
                }
                else if (string.Equals(action, "associate", StringComparison.InvariantCultureIgnoreCase))
                {
                    InstallFileAssociations();
                }
                else if (string.Equals(action, "run", StringComparison.InvariantCultureIgnoreCase))
                {
                    var filePath = args[1];
                    RunScript(filePath, null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Error occured in launcher");
                Environment.ExitCode = 1;
            }
            System.Diagnostics.Trace.WriteLine($"Remotus launcher, exit code: {Environment.ExitCode}");
        }
        

        private static void InstallFileAssociations()
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


        private static void RunScript(string filePath, object parameters)
        {
            var serializer = new XmlSerializer(typeof(Script));
            if (!_fileSystem.FileExists(filePath))
                throw new FileNotFoundException("Could not find script file", filePath);

            Script script;
            using (var stream = _fileSystem.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var obj = serializer.Deserialize(stream);
                script = (Script)obj;
            }
            System.Diagnostics.Trace.WriteLine($"Script loaded: {script?.Name}/{script?.Version} [#{script?.ID}]");

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
            var response = executor.Execute(script, parameterCollection).WaitForResult();
            System.Diagnostics.Trace.WriteLine($"Script executed: {response?.ResultType} | {response?.Result}");
        }


        private static void StartPlugin(string filePath)
        {
            try
            {
                _log.Info(() => $"Starting plugin @{filePath}");
                PluginManager pluginManager;
                using (pluginManager = new PluginManager(_fileSystem))
                {
                    var task = pluginManager.Run(filePath);
                    var source = new CancellationTokenSource();
                    CancellationToken cancellationToken = source.Token;

                    AppDomain.CurrentDomain.DomainUnload += delegate (object sender, EventArgs args)
                    {
                        source.Cancel(true);
                        pluginManager?.Dispose();
                    };
                    task.Wait(cancellationToken);
                }
                pluginManager = null;

                _log.Info(() => $"Plugin exited @{filePath}");
            }
            catch (Exception ex)
            {
                
            }
        }

    }
}
