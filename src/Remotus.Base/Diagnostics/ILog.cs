using System;

namespace FullCtrl.Base
{
    public interface ILog
    {
        void InitializeFor(string loggerName);
        void Debug(string message, params object[] formatting);
        void Debug(Func<string> message);
        void Info(string message, params object[] formatting);
        void Info(Func<string> message);
        void Warn(string message, params object[] formatting);
        void Warn(Func<string> message);
        void Error(string message, params object[] formatting);
        void Error(Func<string> message);
        void Error(Func<string> message, Exception exception);
        void Fatal(string message, params object[] formatting);
        void Fatal(Func<string> message);
        void Fatal(Func<string> message, Exception exception);
    }
}
