using System.Collections.Generic;

namespace Remotus.Base
{
    public interface IResponseMetadata
    {
        IDictionary<string, ILink> Links { get; }
    }
}
