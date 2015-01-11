using System;

namespace ProcHelper
{
    public class ConsoleLogger : ILog
    {
        public void InitializeFor(string loggerName)
        {

        }

        public void Debug(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            msg = "DEBUG: " + msg;
            Console.WriteLine(msg);
        }

        public void Debug(Func<string> message)
        {
            var msg = message();
            msg = "DEBUG: " + msg;
            Console.WriteLine(msg);
        }

        public void Info(string message, params object[] formatting)
        {
            message = "INFO: " + message;
            Console.WriteLine(message, formatting);
        }

        public void Info(Func<string> message)
        {
            var msg = message();
            msg = "INFO: " + msg;
            Console.WriteLine(msg);
        }

        public void Warn(string message, params object[] formatting)
        {
            message = "WARN: " + message;
            Console.WriteLine(message, formatting);
        }

        public void Warn(Func<string> message)
        {
            var msg = message();
            msg = "WARN: " + msg;
            Console.WriteLine(msg);
        }

        public void Error(string message, params object[] formatting)
        {
            message = "ERROR: " + message;
            Console.WriteLine(message, formatting);
        }

        public void Error(Func<string> message)
        {
            var msg = message();
            msg = "ERROR: " + msg;
            Console.WriteLine(msg);
        }

        public void Error(Func<string> message, Exception exception)
        {
            var msg = message();
            msg = "ERROR: " + msg;
            Console.WriteLine(msg, exception.Message);
        }

        public void Fatal(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            msg = "FATAL: " + msg;
            Console.WriteLine(msg);
        }

        public void Fatal(Func<string> message)
        {
            var msg = message();
            msg = "FATAL: " + msg;
            Console.WriteLine(msg);
        }

        public void Fatal(Func<string> message, Exception exception)
        {
            var msg = message();
            msg = "FATAL: " + msg;
            Console.WriteLine(msg, exception.Message);
        }
    }
}
