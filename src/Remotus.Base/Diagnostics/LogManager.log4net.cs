using System;
using System.IO;

namespace Remotus.Base
{
    /// <summary>
    /// Logger type initialization
    /// </summary>
    public static partial class LogManager
    {
        public static void ConfigureLog4Net()
        {
            var configPath = "Log4net.config";
            if (!Path.IsPathRooted(configPath))
                configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath);
            var configFile = new FileInfo(configPath);
            //if (!configFile.Exists)
            //{
            //    var msg = string.Format("Diagnostics config file could not be found at: '{0}'", configFile.FullName);
            //    //throw new InvalidOperationException(msg);
            //    System.Diagnostics.Trace.WriteLine(msg);
            //}

            var config = configFile.Exists
                ? log4net.Config.XmlConfigurator.ConfigureAndWatch(configFile)
                : log4net.Config.XmlConfigurator.Configure();
            if (config == null)
                throw new InvalidOperationException("Could not configure Log4Net.");
        }
    }
}
