using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotus.Base;

namespace Remotus.Plugins.Spotify
{
    public class PlayUriFunction : IFunction<StatusResponse>
    {
        private ModelConverter _modelConverter = new ModelConverter();

        public PlayUriFunction()
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
                var uri = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.Uri)?.Value;
                if (string.IsNullOrWhiteSpace(uri))
                    throw new ArgumentException("Invalid Spotify uri");
                var playContext = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.Context)?.Value;

                Worker.Api.PlayURL(uri, playContext);
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
            public string ID => "43A34F59-7DE0-4796-800E-1641BB91A26B";
            public string Name => "Play uri";
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

            public IFunction<StatusResponse> Instantiate()
            {
                return new PlayUriFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                Uri = new Parameter<string>
                {
                    Name = ParameterKeys.Uri,
                    Required = true,
                    Type = typeof(string),
                    Value = null,
                };
                Context = new Parameter<string>
                {
                    Name = ParameterKeys.Context,
                    Required = false,
                    Type = typeof(string),
                    Value = null,
                };
            }

            public IParameter<string> Uri
            {
                get { return this.GetOrDefault<string>(ParameterKeys.Uri); }
                private set { this[ParameterKeys.Uri] = value; }
            }

            public IParameter<string> Context
            {
                get { return this.GetOrDefault<string>(ParameterKeys.Context); }
                private set { this[ParameterKeys.Context] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string Uri = "Uri";
            public const string Context = "Context";
        }

        public void Dispose()
        {
            
        }
    }
}
