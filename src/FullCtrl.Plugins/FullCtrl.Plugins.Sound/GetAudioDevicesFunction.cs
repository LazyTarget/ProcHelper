using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using FullCtrl.Base;
using Lux;
using Lux.Interfaces;

namespace FullCtrl.Plugins.Sound
{
    public class GetAudioDevicesFunction : IFunctionDescriptor, IFunction
    {
        private CoreAudioController _audioController = new CoreAudioController();
        private ModelConverter _modelConverter = new ModelConverter();
        private IConverter _converter = new Converter();

        public GetAudioDevicesFunction()
        {
            
        }

        public string Name => nameof(GetAudioDevicesFunction);

        public IParameterCollection GetParameters()
        {
            var res = new ParameterCollection();
            res[ParameterKeys.DeviceType] = new Parameter
            {
                Name = ParameterKeys.DeviceType,
                Required = false,
                Type = typeof(AudioDeviceType?),
                Value = null,
            };
            res[ParameterKeys.DeviceState] = new Parameter
            {
                Name = ParameterKeys.DeviceState,
                Required = false,
                Type = typeof(AudioDeviceState?),
                Value = null,
            };
            return res;
        }

        IFunction IFunctionDescriptor.Instantiate()
        {
            return this;
        }
        
        public async Task<IFunctionResult> Execute(IFunctionArguments arguments)
        {
            try
            {
                //AudioDeviceType? deviceType = _converter.Convert<AudioDeviceType?>(arguments?.Parameters?["DeviceType"]?.Value);
                //AudioDeviceState? deviceState = _converter.Convert<AudioDeviceState?>(arguments?.Parameters?["DeviceState"]?.Value);

                //AudioSwitcher.AudioApi.DeviceType? deviceType2 = deviceType.HasValue
                //    ? _converter.Convert<AudioSwitcher.AudioApi.DeviceType?>((int) deviceType.Value)
                //    : null;
                //AudioSwitcher.AudioApi.DeviceState? deviceState2 = deviceState.HasValue
                //    ? _converter.Convert<AudioSwitcher.AudioApi.DeviceState?>((int)deviceState.Value)
                //    : null;

                AudioSwitcher.AudioApi.DeviceType? deviceType2 = arguments?.Parameters.GetParamValue<AudioSwitcher.AudioApi.DeviceType?>(ParameterKeys.DeviceType);
                AudioSwitcher.AudioApi.DeviceState? deviceState2 = arguments?.Parameters.GetParamValue<AudioSwitcher.AudioApi.DeviceState?>(ParameterKeys.DeviceType);


                IEnumerable<CoreAudioDevice> devices;
                if (deviceType2.HasValue && deviceState2.HasValue)
                {
                    devices = _audioController.GetDevices(deviceType2.Value, deviceState2.Value);
                }
                else if (deviceType2.HasValue)
                {
                    devices = _audioController.GetDevices(deviceType2.Value);
                }
                else if (deviceState2.HasValue)
                {
                    devices = _audioController.GetDevices(deviceState2.Value);
                }
                else
                {
                    devices = _audioController.GetDevices();
                }
                var res = devices?.Select(_modelConverter.FromAudioDevice).Where(x => x != null).ToList();

                var result = new FunctionResult();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public static class ParameterKeys
        {
            public const string DeviceType = "DeviceType";
            public const string DeviceState = "DeviceState";
        }

    }
}
