using System;

namespace Remotus.API.Hubs.Client
{
    public class HubRequest
    {
        public HubMessage Message { get; set; }
        public Action<object> OnProgress { get; set; }
    }
}
