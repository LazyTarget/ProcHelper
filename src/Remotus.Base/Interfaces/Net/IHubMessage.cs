using System.Collections.Generic;

namespace Remotus.Base.Interfaces.Net
{
    public interface IHubMessage
    {
        string Method { get; }

        IReadOnlyList<object> Args { get; }

        bool Queuable { get; }
    }
}
