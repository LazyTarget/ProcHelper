using System;

namespace Remotus.Base
{
    public class Parameter : IParameter
    {
        public string Name { get; set; }
        public bool Required { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }
}