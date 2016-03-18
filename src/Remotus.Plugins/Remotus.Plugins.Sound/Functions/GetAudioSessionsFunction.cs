using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class GetAudioSessionsFunction : IFunctionDescriptor, IFunction, IFunction<IList<AudioSession>>
    {
        private CoreAudioController _audioController = new CoreAudioController();
        private ModelConverter _modelConverter = new ModelConverter();

        public GetAudioSessionsFunction()
        {
            
        }

        public string Name => nameof(GetAudioSessionsFunction);

        public IParameterCollection GetParameters()
        {
            var res = new ParameterCollection();
            res[ParameterKeys.DeviceID] = new Parameter
            {
                Name = ParameterKeys.DeviceID,
                Required = false,
                Type = typeof(string),
                Value = null,
            };
            return res;
        }

        IFunction IFunctionDescriptor.Instantiate()
        {
            return this;
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
                var deviceID = arguments?.Parameters.GetParamValue<string>(ParameterKeys.DeviceID);

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
