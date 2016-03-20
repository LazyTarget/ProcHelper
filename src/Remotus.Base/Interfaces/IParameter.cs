using System;

namespace Remotus.Base
{
    public interface IParameter
    {
        string Name { get; }
        bool Required { get; }
        Type Type { get; }
        object Value { get; set; }
    }

    public interface IParameter<TValue> : IParameter
    {
        new TValue Value { get; set; }
    }
}