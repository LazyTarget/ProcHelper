using System;

namespace Sandbox.Console
{
    public class ReconnectArguments
    {
        public ReconnectArguments()
        {
            AutoReconnect = true;
            ReconnectInterval = TimeSpan.FromSeconds(5);
            ConnectTimeout = TimeSpan.FromSeconds(30);
        }

        public bool AutoReconnect { get; set; }
        public TimeSpan ReconnectInterval { get; set; }
        public TimeSpan ConnectTimeout { get; set; }
    }
}