using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Remotus.Base.Models.Payloads;

namespace Remotus.API.Hubs
{
    public class AgentHub : HubBase
    {
        // Events:
        // OnPluginStatusChanged(PluginStatusChanged model)


        public void OnPluginStatusChanged(PluginStatusChanged model)
        {
            Clients.All.OnPluginStatusChanged(model);
        }

    }
}
