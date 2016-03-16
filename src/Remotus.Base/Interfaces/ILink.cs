using System;

namespace FullCtrl.Base
{
    public interface ILink
    {
        string Href { get; }
        string Relative { get; }

        Uri ToUri();
    }
}
