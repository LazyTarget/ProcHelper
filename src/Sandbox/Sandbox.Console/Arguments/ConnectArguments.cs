using System;
using System.Collections.Generic;

namespace Sandbox.Console
{
    public class ConnectArguments
    {
        public ConnectArguments()
        {
            Host = "localhost";
            Port = 9000;
            Timeout = TimeSpan.FromSeconds(30);
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public List<string> Hubs { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}