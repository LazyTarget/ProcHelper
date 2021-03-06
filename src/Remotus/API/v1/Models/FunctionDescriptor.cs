﻿using Remotus.Base;

namespace Remotus.API.v1
{
    public class FunctionDescriptor : IFunctionDescriptor
    {
        public FunctionDescriptor()
        {
            
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public IParameterCollection Parameters { get; set; }

        public IParameterCollection GetParameters()
        {
            return Parameters;
        }

        public IFunction Instantiate()
        {
            throw new System.NotSupportedException();
        }
    }
}
