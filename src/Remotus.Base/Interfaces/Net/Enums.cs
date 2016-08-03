using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remotus.Base.Net
{
    // Copied from: Assembly Microsoft.AspNet.SignalR.Client, Version=2.2.0.0
    public enum ConnectionState
    {
        Connecting = 0,
        Connected = 1,
        Reconnecting = 2,
        Disconnected = 3
    }
}
