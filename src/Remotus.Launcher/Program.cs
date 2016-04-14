using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Remotus.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Trace.WriteLine($"Remotus launcher!!! " +
                                               $"UserInteractive: {Environment.UserInteractive}. " +
                                               $"Command line: {Environment.CommandLine} " +
                                               $"Args: {String.Join(" ", args)}");

            // todo: install file associations, pointing to this exe (Assembly.GetExecutingAssembly().Location)
            // todo: open .remotus files
        }

        private static void InstallAssociations()
        {
            //BrendanGrant.Helpers.FileAssociation.
        }
    }
}
