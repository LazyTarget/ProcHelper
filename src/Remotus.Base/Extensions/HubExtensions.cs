using System;
using System.Threading.Tasks;
using Remotus.Base.Interfaces.Net;

namespace Remotus.Base
{
    public static class HubExtensions
    {
        public static void ConnectContinuous(this IHubConnector connector)
        {
            Task task = null;
            var timeout = TimeSpan.FromSeconds(20);
            try
            {
                task = connector?.Connect();
                task?.Wait(timeout);
            }
            catch (Exception ex)
            {

            }

            connector?.EnsureReconnecting();
        }

    }
}