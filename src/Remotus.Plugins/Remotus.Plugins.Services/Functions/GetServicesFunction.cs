using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Services
{
    public class GetServicesFunction : IFunction<IList<WinServiceDto>>
    {
        private readonly IWinServiceHelper _serviceHelper = new WinServiceHelper();

        public GetServicesFunction()
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

        public async Task<IFunctionResult<IList<WinServiceDto>>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var res = _serviceHelper.GetServices();

                var result = new FunctionResult<IList<WinServiceDto>>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<IList<WinServiceDto>>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "1DC6332C-FEE0-4AC4-8F22-A649657F60A2";
            public string Name => "Get services";
            public string Version => "1.0.0.0";

            public IParameterCollection GetParameters()
            {
                IParameterCollection res = null;
                return res;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<WinServiceDto> Instantiate()
            {
                return new GetServiceFunction();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
