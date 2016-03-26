using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using Lux;
using Lux.Interfaces;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class SetDefaultAudioDeviceFunction : IFunction, IFunction<AudioDevice>
    {
        private CoreAudioController _audioController = new CoreAudioController();
        private ModelConverter _modelConverter = new ModelConverter();
        private IConverter _converter = new Converter();

        public SetDefaultAudioDeviceFunction()
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

        public async Task<IFunctionResult<AudioDevice>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                AudioSwitcher.AudioApi.CoreAudio.CoreAudioDevice device;
                var deviceID = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.DeviceID)?.Value;
                var guid = _converter.Convert<Guid>(deviceID);
                if (guid != Guid.Empty)
                {
                    device = await _audioController.GetDeviceAsync(guid);
                    var r = await device.SetAsDefaultAsync();
                    if (!r)
                        throw new Exception($"Request resulted with: '{r}'");
                }
                else
                    device = null;

                var res = _modelConverter.FromAudioDevice(device);

                var result = new FunctionResult<AudioDevice>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<AudioDevice>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string ID => "2C7B8619-1642-41C4-97F3-FD08E46FE430";
            public string Name => "Set default device";
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

            public IFunction<AudioDevice> Instantiate()
            {
                return new SetDefaultAudioDeviceFunction();
            }
        }

        public class Parameters : ParameterCollection
        {
            public Parameters()
            {
                DeviceID = new Parameter<string>
                {
                    Name = ParameterKeys.DeviceID,
                    Required = true,
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
            _audioController?.Dispose();
            _audioController = null;
        }
    }
}
