﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using Lux;
using Lux.Interfaces;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class GetAudioDevicesFunction : IFunction, IFunction<IList<AudioDevice>>
    {
        private CoreAudioController _audioController = new CoreAudioController();
        private ModelConverter _modelConverter = new ModelConverter();
        private IConverter _converter = new Converter();

        public GetAudioDevicesFunction()
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
                
                var deviceType = arguments?.Parameters.GetOrDefault<AudioSwitcher.AudioApi.DeviceType?>(ParameterKeys.DeviceType)?.Value;
                var deviceState = arguments?.Parameters.GetOrDefault<AudioSwitcher.AudioApi.DeviceState?>(ParameterKeys.DeviceState)?.Value;


                IEnumerable<CoreAudioDevice> devices;
                if (deviceType.HasValue && deviceState.HasValue)
                {
                    devices = _audioController?.GetDevices(deviceType.Value, deviceState.Value);
                }
                else if (deviceType.HasValue)
                {
                    devices = _audioController?.GetDevices(deviceType.Value);
                }
                else if (deviceState.HasValue)
                {
                    devices = _audioController?.GetDevices(deviceState.Value);
                }
                else
                {
                    devices = _audioController?.GetDevices();
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


        public class Descriptor : IFunctionDescriptor
        {
            public string Name => nameof(GetAudioDevicesFunction);
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

            public IFunction<IList<AudioDevice>> Instantiate()
            {
                return new GetAudioDevicesFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                DeviceType = new Parameter<AudioDeviceType?>
                {
                    Name = ParameterKeys.DeviceType,
                    Required = false,
                    Type = typeof(AudioDeviceType?),
                    Value = null,
                };
                DeviceState = new Parameter<AudioDeviceState?>
                {
                    Name = ParameterKeys.DeviceState,
                    Required = false,
                    Type = typeof(AudioDeviceState?),
                    Value = null,
                };
            }

            public IParameter<AudioDeviceType?> DeviceType
            {
                get { return this.GetOrDefault<AudioDeviceType?>(ParameterKeys.DeviceType); }
                private set { this[ParameterKeys.DeviceType] = value; }
            }

            public IParameter<AudioDeviceState?> DeviceState
            {
                get { return this.GetOrDefault<AudioDeviceState?>(ParameterKeys.DeviceState); }
                private set { this[ParameterKeys.DeviceState] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string DeviceType = "DeviceType";
            public const string DeviceState = "DeviceState";
        }

        public void Dispose()
        {
            _audioController?.Dispose();
            _audioController = null;
        }
    }
}
