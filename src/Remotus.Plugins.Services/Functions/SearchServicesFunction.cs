using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Services
{
    public class SearchServicesFunction : IFunction<IList<WinServiceDto>>
    {
        private readonly IWinServiceHelper _serviceHelper = new WinServiceHelper();

        public SearchServicesFunction()
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
            public string ID => "06573BBD-5054-480C-B6B8-A4B27C297EC7";
            public string Name => "Search services";
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

            public IFunction<IList<WinServiceDto>> Instantiate()
            {
                return new SearchServicesFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                Query = new Parameter<string>
                {
                    Name = ParameterKeys.Query,
                    Required = true,
                    Type = typeof(string),
                    Value = null,
                };
            }
            
            public IParameter<string> Query
            {
                get { return this.GetOrDefault<string>(ParameterKeys.Query); }
                private set { this[ParameterKeys.Query] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string Query = "Query";
        }

        public void Dispose()
        {
            
        }
    }
}
