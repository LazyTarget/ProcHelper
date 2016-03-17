using System;

namespace Remotus.Base
{
    public interface ILink
    {
        string Href { get; }
        string Relative { get; }

        Uri ToUri();
    }
}
