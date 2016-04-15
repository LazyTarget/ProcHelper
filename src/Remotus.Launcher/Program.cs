using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Lux.Extensions;
using Lux.IO;
using Remotus.API;
using Remotus.Base;
using Remotus.Base.Scripting;

namespace Remotus.Launcher
{
    class Program
    {
        private static IFileSystem _fileSystem = new FileSystem();

        static void Main(string[] args)
        {
            System.Diagnostics.Trace.WriteLine($"Remotus launcher!!! " +
                                               $"UserInteractive: {Environment.UserInteractive}. " +
                                               $"Command line: {Environment.CommandLine} " +
                                               $"Args: {String.Join(" ", args)}");

            // todo: install file associations, pointing to this exe (Assembly.GetExecutingAssembly().Location)
            // todo: open .remotus files

            var action = args[0];
            if (action == "run")
            {
                var filePath = args[1];
                RunScript(filePath, null);
            }
        }

        private static void InstallAssociations()
        {
            //BrendanGrant.Helpers.FileAssociation.
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
                Remotus = new Remotus.API.v1.FullCtrlAPI(),
            };

            var parameterCollection = new ParameterCollection();
            // todo: populate parameters

            // todo: execute via API, so that Remotus.API is the one doing the executing
            var response = executor.Execute(script, parameterCollection).WaitForResult();
            System.Diagnostics.Trace.WriteLine($"Script executed: {response?.ResultType} | {response?.Result}");
        }
    }
}
