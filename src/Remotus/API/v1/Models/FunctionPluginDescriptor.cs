﻿using System.Collections.Generic;
using Remotus.Base;

namespace Remotus.API.v1
{
    public class FunctionPluginDescriptor : IFunctionPlugin
    {
        public FunctionPluginDescriptor()
        {

        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public IFunctionDescriptor[] Functions { get; set; }

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            return Functions;
        }

        public void Dispose()
        {

        }
    }
}
