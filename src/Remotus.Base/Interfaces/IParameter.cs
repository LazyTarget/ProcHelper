using System;

namespace FullCtrl.Base
{
    public interface IParameter
    {
        string Name { get; set; }
        bool Required { get; set; }
        Type Type { get; set; }
        object Value { get; set; }
    }
}