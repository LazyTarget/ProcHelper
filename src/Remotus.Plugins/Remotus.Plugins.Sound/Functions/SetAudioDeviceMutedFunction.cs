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
    public class SetAudioDeviceMutedFunction : IFunction, IFunction<AudioDevice>
    {
        private CoreAudioController _audioController = SoundPlugin.AudioController;
        private ModelConverter _modelConverter = new ModelConverter();
        private IConverter _converter = new Converter();

        public SetAudioDeviceMutedFunction()
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
                    device = await _audioController.GetDeviceAsync(guid);
                else
                    device = _audioController.DefaultPlaybackDevice;
                if (device == null)
                    throw new Exception("Device not found");

                var muted = arguments?.Parameters.GetOrDefault<bool>(ParameterKeys.Muted)?.Value;
                if (!muted.HasValue)
                    throw new ArgumentException("Missing volume argument");

                await device.MuteAsync(muted.Value);

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
            public string ID => "F660E792-1D59-4E45-A9A1-E0F62674533D";
            public string Name => "Set audio device muted";
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
                return new SetAudioDeviceMutedFunction();
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
                Muted = new Parameter<bool>
                {
                    Name = ParameterKeys.Muted,
                    Required = true,
                    Type = typeof(bool),
                    Value = false,
                };
            }

            public IParameter<string> DeviceID
            {
                get { return this.GetOrDefault<string>(ParameterKeys.DeviceID); }
                private set { this[ParameterKeys.DeviceID] = value; }
            }

            public IParameter<bool> Muted
            {
                get { return this.GetOrDefault<bool>(ParameterKeys.Muted); }
                private set { this[ParameterKeys.Muted] = value; }
            }
        }

        public static class ParameterKeys
        {
            public const string DeviceID = "DeviceID";
            public const string Muted = "Muted";
        }

        public void Dispose()
        {
            //_audioController?.Dispose();
            //_audioController = null;
        }
    }
}
