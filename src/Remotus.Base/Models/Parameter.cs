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

    public class Parameter<TValue> : Parameter, IParameter<TValue>
    {
        public Parameter()
        {
            Type = typeof (TValue);
            Value = default(TValue);
        } 

        public new TValue Value
        {
            get { return (TValue) base.Value; }
            set { base.Value = value; }
        }


        public static Parameter<TValue> Create(IParameter parameter)
        {
            if (parameter == null)
                return null;
            var result = new Parameter<TValue>
            {
                Name = parameter.Name,
                Required = parameter.Required,
                Type = parameter.Type,
            };

            var converter = new Lux.Converter();
            //result.Value = (TValue) parameter.Value;
            result.Value = converter.Convert<TValue>(parameter.Value);
            return result;
        }
    }
}