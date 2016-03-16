using System.Collections.Generic;

namespace FullCtrl.Base
{
    public interface IResponseMetadata
    {
        IDictionary<string, ILink> Links { get; }
    }
}
