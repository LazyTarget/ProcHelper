using System;
using System.Diagnostics;

namespace ProcHelper
{
    public class TraceLogger : ILog
    {
        public void InitializeFor(string loggerName)
        {

        }

        public void Debug(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            msg = "DEBUG: " + msg;
            Trace.WriteLine(msg);
        }

        public void Debug(Func<string> message)
        {
            var msg = message();
            msg = "DEBUG: " + msg;
            Trace.WriteLine(msg);
        }

        public void Info(string message, params object[] formatting)
        {
            message = "INFO: " + message;
            Trace.TraceInformation(message, formatting);
        }

        public void Info(Func<string> message)
        {
            var msg = message();
            msg = "INFO: " + msg;
            Trace.TraceInformation(msg);
        }

        public void Warn(string message, params object[] formatting)
        {
            message = "WARN: " + message;
            Trace.TraceWarning(message, formatting);
        }

        public void Warn(Func<string> message)
        {
            var msg = message();
            msg = "WARN: " + msg;
            Trace.TraceWarning(msg);
        }

        public void Error(string message, params object[] formatting)
        {
            message = "ERROR: " + message;
            Trace.TraceError(message, formatting);
        }

        public void Error(Func<string> message)
        {
            var msg = message();
            msg = "ERROR: " + msg;
            Trace.TraceError(msg);
        }

        public void Error(Func<string> message, Exception exception)
        {
            var msg = message();
            msg = "ERROR: " + msg;
            Trace.TraceError(msg, exception.Message);
        }

        public void Fatal(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            msg = "FATAL: " + msg;
            Trace.Fail(msg);
        }

        public void Fatal(Func<string> message)
        {
            var msg = message();
            msg = "FATAL: " + msg;
            Trace.Fail(msg);
        }

        public void Fatal(Func<string> message, Exception exception)
        {
            var msg = message();
            msg = "FATAL: " + msg;
            Trace.Fail(msg, exception.Message);
        }
    }
}
