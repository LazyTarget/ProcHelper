using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class GetStatusFunction : IFunction<StatusResponse>
    {
        private readonly ModelConverter _modelConverter = new ModelConverter();

        public GetStatusFunction()
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

        public async Task<IFunctionResult<StatusResponse>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var connected = Worker.ConnectIfNotConnected();
                if (!connected)
                {
                    throw new Exception("Unable to connect to Spotify");
                }
                var status = Worker.Api.GetStatus();
                var res = _modelConverter.FromStatusResponse(status);

                var result = new FunctionResult<StatusResponse>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<StatusResponse>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "1EE535A9-382C-4BE6-8EEC-8E7B039F6F37";
            public string Name => "Get status";
            public string Version => "1.0.0.0";

            public IParameterCollection GetParameters()
            {
                IParameterCollection parameterCollection = null;
                return parameterCollection;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<StatusResponse> Instantiate()
            {
                return new GetStatusFunction();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
