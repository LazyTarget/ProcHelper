using System.Collections.Generic;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1
{
    [JsonConverter(typeof(FunctionPluginConverter))]
    public class FunctionPluginDescriptor : IFunctionPlugin
    {
        public FunctionPluginDescriptor()
        {

        }

        public string Name { get; set; }
        public string Version { get; set; }
        public IFunctionDescriptor[] Functions { get; set; }

        public IEnumerable<IFunctionDescriptor> GetFunctions()
        {
            return Functions;
        }
    }
}
