using System;
using System.Threading.Tasks;

namespace Remotus.Base
{
    public static class JsonExtensions
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