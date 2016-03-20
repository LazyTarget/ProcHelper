using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class GetAudioSessionsFunction : IFunction, IFunction<IList<AudioSession>>
    {
        private CoreAudioController _audioController = new CoreAudioController();
        private ModelConverter _modelConverter = new ModelConverter();

        public GetAudioSessionsFunction()
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

        public async Task<IFunctionResult<IList<AudioSession>>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                var deviceID = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.DeviceID)?.Value;

                Guid guid;
                CoreAudioDevice device = null;
                if (string.IsNullOrEmpty(deviceID))
                {
                    device = _audioController.DefaultPlaybackDevice;
                }
                else if (Guid.TryParse(deviceID, out guid))
                {
                    device = _audioController.GetDevice(guid);
                }


                if (device == null)
                {
                    throw new Exception("Device not found");
                }

                var sessions = device?.SessionController?.All();
                var res = sessions?.Select(_modelConverter.FromAudioControllerState).Where(x => x != null).ToList();

                var result = new FunctionResult<IList<AudioSession>>
                {
                    Arguments = arguments,
                    Result = res,
                };
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<IList<AudioSession>>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "D5F99DAC-03A7-4E33-878C-20CA93726DE9";
            public string Name => nameof(GetAudioSessionsFunction);
            public string Version => "1.0.0.0";

            IParameterCollection IFunctionDescriptor.GetParameters()
            {
                return GetParameters();
            }

            public Parameters GetParameters()
            {
                var res = new Parameters();
                return res;
            }

            IFunction IComponentInstantiator<IFunction>.Instantiate()
            {
                return Instantiate();
            }

            public IFunction<IList<AudioSession>> Instantiate()
            {
                return new GetAudioSessionsFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                DeviceID = new Parameter<string>
                {
                    Name = ParameterKeys.DeviceID,
                    Required = false,
                    Type = typeof(string),
                    Value = null,
                };
            }

            public IParameter<string> DeviceID
            {
                get { return this.GetOrDefault<string>(ParameterKeys.DeviceID); }
                private set { this[ParameterKeys.DeviceID] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string DeviceID = "DeviceID";
        }

        public void Dispose()
        {
            _audioController.Dispose();
        }

    }
}
