using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Services
{
    public class StartServiceFunction : IFunction<WinServiceDto>
    {
        private readonly IWinServiceHelper _serviceHelper = new WinServiceHelper();

        public StartServiceFunction()
        {
            
        }

        public IFunctionDescriptor GetDescriptor()
        {
            return new Descriptor();
        }

        async Task<IFunctionResult> IFunction.Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            var result = await Execute(context, arguments);
            return result;
        }

        public async Task<IFunctionResult<WinServiceDto>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var serviceName = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.ServiceName)?.Value;
                var res = _serviceHelper.StartService(serviceName);

                var result = new FunctionResult<WinServiceDto>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<WinServiceDto>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "6D71B04B-30B0-4770-9ADC-BDDE22847955";
            public string Name => "Start service";
            public string Version => "1.0.0.0";

            public IParameterCollection GetParameters()
            {
                var res = new Parameters();
                return res;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<WinServiceDto> Instantiate()
            {
                return new StartServiceFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                ServiceName = new Parameter<string>
                {
                    Name = ParameterKeys.ServiceName,
                    Required = true,
                    Type = typeof(string),
                    Value = null,
                };
            }
            
            public IParameter<string> ServiceName
            {
                get { return this.GetOrDefault<string>(ParameterKeys.ServiceName); }
                private set { this[ParameterKeys.ServiceName] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string ServiceName = "ServiceName";
        }

        public void Dispose()
        {
            
        }
    }
}
