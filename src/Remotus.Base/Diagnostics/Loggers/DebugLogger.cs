using System;
using System.Diagnostics;

namespace Remotus.Base
{
    public class DebugLogger : ILog
    {
        public void InitializeFor(string loggerName)
        {

        }

        public void Debug(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            msg = "DEBUG: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Debug(Func<string> message)
        {
            var msg = message();
            msg = "DEBUG: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Info(string message, params object[] formatting)
        {
            message = "INFO: " + message;
            System.Diagnostics.Debug.WriteLine(message, formatting);
        }

        public void Info(Func<string> message)
        {
            var msg = message();
            msg = "INFO: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Warn(string message, params object[] formatting)
        {
            message = "WARN: " + message;
            System.Diagnostics.Debug.WriteLine(message, formatting);
        }

        public void Warn(Func<string> message)
        {
            var msg = message();
            msg = "WARN: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Error(string message, params object[] formatting)
        {
            message = "ERROR: " + message;
            System.Diagnostics.Debug.WriteLine(message, formatting);
        }

        public void Error(Func<string> message)
        {
            var msg = message();
            msg = "ERROR: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Error(Func<string> message, Exception exception)
        {
            var msg = message();
            msg = "ERROR: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(exception.ToString());
        }

        public void Fatal(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            msg = "FATAL: " + msg;
            System.Diagnostics.Debug.Fail(msg);
        }

        public void Fatal(Func<string> message)
        {
            var msg = message();
            msg = "FATAL: " + msg;
            System.Diagnostics.Debug.Fail(msg);
        }

        public void Fatal(Func<string> message, Exception exception)
        {
            var msg = message();
            msg = "FATAL: " + msg;
            System.Diagnostics.Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine(exception.ToString());
        }
    }
}
