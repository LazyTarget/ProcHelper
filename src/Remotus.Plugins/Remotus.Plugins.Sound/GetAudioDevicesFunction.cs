﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using FullCtrl.Base;
using Lux;
using Lux.Interfaces;

namespace FullCtrl.Plugins.Sound
{
    public class GetAudioDevicesFunction : IFunctionDescriptor, IFunction, IFunction<IList<AudioDevice>>
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

        async Task<IFunctionResult> IFunction.Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            var result = await Execute(context, arguments);
            return result;
        }

        public async Task<IFunctionResult<IList<AudioDevice>>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
#if DEBUG
                if (context?.ClientInfo != null)
                {
                    var plugs = await context.ClientInfo.GetPlugins();
                    var l = plugs?.ToList();
                }
#endif

                var deviceType = arguments?.Parameters.GetParamValue<AudioSwitcher.AudioApi.DeviceType?>(ParameterKeys.DeviceType);
                var deviceState = arguments?.Parameters.GetParamValue<AudioSwitcher.AudioApi.DeviceState?>(ParameterKeys.DeviceState);


                IEnumerable<CoreAudioDevice> devices;
                if (deviceType.HasValue && deviceState.HasValue)
                {
                    devices = _audioController.GetDevices(deviceType.Value, deviceState.Value);
                }
                else if (deviceType.HasValue)
                {
                    devices = _audioController.GetDevices(deviceType.Value);
                }
                else if (deviceState.HasValue)
                {
                    devices = _audioController.GetDevices(deviceState.Value);
                }
                else
                {
                    devices = _audioController.GetDevices();
                }
                var res = devices?.Select(_modelConverter.FromAudioDevice).Where(x => x != null).ToList();

                var result = new FunctionResult<IList<AudioDevice>>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<IList<AudioDevice>>();
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

        public void Dispose()
        {
            _audioController.Dispose();
        }
    }
}