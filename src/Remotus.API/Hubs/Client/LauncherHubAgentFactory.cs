using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNet.SignalR.Client;

namespace Remotus.API.Hubs.Client
{
    public class LauncherHubAgentFactory : HubAgentFactory
    {
        public LauncherHubAgentFactory()
            : base()
        {
        }
        
        protected override HubConnection CreateConnection(ICredentials credentials, IDictionary<string, string> queryString = null)
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            var connection = base.CreateConnection(credentials, queryString);
            connection.Headers["App-UA-Type"] = $"Remotus.Launcher.exe/{version}";
            connection.Headers["Process-ID"] = Process.GetCurrentProcess()?.Id.ToString();
            return connection;
        }

    }
}
