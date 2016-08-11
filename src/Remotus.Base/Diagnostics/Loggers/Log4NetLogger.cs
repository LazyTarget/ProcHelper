using System;
using System.Diagnostics;
using System.Reflection;

namespace Remotus.Base
{
    public class Log4NetLogger : ILog
    {
        private log4net.ILog _log;

        public Log4NetLogger()
        {
            _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public void InitializeFor(string loggerName)
        {
            _log = log4net.LogManager.GetLogger(loggerName);
        }

        public void Debug(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            _log.Debug(msg);
        }

        public void Debug(Func<string> message)
        {
            var msg = message();
            _log.Debug(msg);
        }

        public void Info(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            _log.Info(msg);
        }

        public void Info(Func<string> message)
        {
            var msg = message();
            _log.Info(msg);
        }

        public void Warn(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            _log.Warn(msg);
        }

        public void Warn(Func<string> message)
        {
            var msg = message();
            _log.Warn(msg);
        }

        public void Error(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            _log.Error(msg);
        }

        public void Error(Func<string> message)
        {
            var msg = message();
            _log.Error(msg);
        }

        public void Error(Func<string> message, Exception exception)
        {
            var msg = message();
            _log.Error(msg, exception);
        }

        public void Fatal(string message, params object[] formatting)
        {
            var msg = string.Format(message, formatting);
            _log.Fatal(msg);
        }

        public void Fatal(Func<string> message)
        {
            var msg = message();
            _log.Fatal(msg);
        }

        public void Fatal(Func<string> message, Exception exception)
        {
            var msg = message();
            _log.Fatal(msg, exception);
        }
    }
}
