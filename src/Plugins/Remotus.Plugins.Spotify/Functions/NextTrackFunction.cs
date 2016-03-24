using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class NextTrackFunction : IFunction<StatusResponse>
    {
        private readonly ModelConverter _modelConverter = new ModelConverter();

        public NextTrackFunction()
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
                Worker.Api.Skip();
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
            public string ID => "78A840E7-6C76-4A8A-B5BF-EFC68E19B945";
            public string Name => "Next track";
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

            public IFunction<StatusResponse> Instantiate()
            {
                return new NextTrackFunction();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
