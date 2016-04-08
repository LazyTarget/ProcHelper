using System;
using System.Collections.Generic;
using System.Linq;

namespace Remotus.Base
{
    public class ResponseMetadata : IResponseMetadata
    {
        public ResponseMetadata()
        {
            Links = new Dictionary<string, ILink>();
            Metadata = new Dictionary<string, object>();
        }

        public IDictionary<string, ILink> Links { get; set; }
        public IDictionary<string, object> Metadata { get; set; }
    }
}
