using System;
using System.Collections.Generic;

namespace Remotus.Base.Models.Hub
{
    public class HubMessage : IHubMessage
    {
        public string Method { get; set; }

        public object[] Args { get; set; }

        public bool Queuable { get; set; }
        

        IReadOnlyList<object> IHubMessage.Args
        {
            get
            {
                return new ArraySegment<object>(Args);
            }
        }
    }
}
