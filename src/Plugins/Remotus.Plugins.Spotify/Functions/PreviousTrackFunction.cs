using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class PreviousTrackFunction : IFunction<StatusResponse>
    {
        private readonly ModelConverter _modelConverter = new ModelConverter();

        public PreviousTrackFunction()
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
                Worker.Api.Previous();
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
            public string ID => "DFEEBD1B-E504-48C4-B371-90730DF49E13";
            public string Name => "Previous track";
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
                return new PreviousTrackFunction();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
