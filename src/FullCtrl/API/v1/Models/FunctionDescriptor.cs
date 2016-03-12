using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1
{
    [JsonConverter(typeof(FunctionDescriptorConverter))]
    public class FunctionDescriptor : IFunctionDescriptor
    {
        public FunctionDescriptor()
        {
            
        }

        public string Name { get; set; }
        public IParameterCollection Parameters { get; set; }

        public IParameterCollection GetParameters()
        {
            return Parameters;
        }

        public IFunction Instantiate()
        {
            throw new System.NotImplementedException();
        }
    }
}
