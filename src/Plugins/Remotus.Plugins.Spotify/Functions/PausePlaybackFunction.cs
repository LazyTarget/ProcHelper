using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class PausePlaybackFunction : IFunction<PlaybackStatus>
    {
        public PausePlaybackFunction()
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

        public async Task<IFunctionResult<PlaybackStatus>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var connected = Worker.ConnectIfNotConnected();
                if (!connected)
                {
                    throw new Exception("Unable to connect to Spotify");
                }
                Worker.Api.Pause();
                var status = Worker.Api.GetStatus();
                var res = new PlaybackStatus
                {
                    Status = status,
                };

                var result = new FunctionResult<PlaybackStatus>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<PlaybackStatus>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "B525CAD9-27E6-4709-B29B-96A32438C7D0";
            public string Name => nameof(PausePlaybackFunction);
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

            public IFunction<PlaybackStatus> Instantiate()
            {
                return new PausePlaybackFunction();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
