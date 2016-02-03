﻿using System;

namespace FullCtrl.API.Interfaces
{
    public interface ILink
    {
        string Href { get; }
        string Relative { get; }

        Uri ToUri();
    }
}
