﻿using System;

namespace FullCtrl.Base
{
    public class NullLogger : ILog
    {
        public void InitializeFor(string loggerName)
        {
        }

        public void Debug(string message, params object[] formatting)
        {
        }

        public void Debug(Func<string> message)
        {
        }

        public void Info(string message, params object[] formatting)
        {
        }

        public void Info(Func<string> message)
        {
        }

        public void Warn(string message, params object[] formatting)
        {
        }

        public void Warn(Func<string> message)
        {
        }

        public void Error(string message, params object[] formatting)
        {
        }

        public void Error(Func<string> message)
        {
        }

        public void Error(Func<string> message, Exception exception)
        {
        }

        public void Fatal(string message, params object[] formatting)
        {
        }

        public void Fatal(Func<string> message)
        {
        }

        public void Fatal(Func<string> message, Exception exception)
        {
        }
    }
}