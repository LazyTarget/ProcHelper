using System;
using System.Collections.Generic;

namespace Sandbox.Console
{
    public class SendToHubArguments
    {
        public SendToHubArguments()
        {
            Timeout = TimeSpan.FromSeconds(30);
        }

        public string HubName { get; set; }
        public string Method { get; set; }
        public List<string> Args { get; set; }
        public TimeSpan Timeout { get; set; }
        public bool Queuable { get; set; }
    }
}
