using System;

namespace Remotus.Base.Models.Hub
{
    public class HubRequest
    {
        public HubMessage Message { get; set; }
        public Action<object> OnProgress { get; set; }
    }
}
