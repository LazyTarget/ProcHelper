using System;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using Lux;
using Lux.Extensions;
using Lux.Interfaces;
using Remotus.Base;

namespace Remotus.Plugins.Sound
{
    public class ToggleMuteAudioDeviceFunction : IFunction, IFunction<object>
    {
        private CoreAudioController _audioController = new CoreAudioController();
        private IConverter _converter = new Converter();

        public ToggleMuteAudioDeviceFunction()
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

        public async Task<IFunctionResult<object>> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                object res = null;
                //var guid = arguments?.Parameters.GetParamValue<Guid>(ParameterKeys.DeviceID);
                var id = arguments?.Parameters.GetOrDefault<string>(ParameterKeys.DeviceID)?.Value;
                var guid = _converter.Convert<Guid>(id);
                if (guid != Guid.Empty)
                {
                    var device = _audioController.GetDevice(guid);
                    if (device != null)
                    {
                        res = device.Mute(!device.IsMuted);
                    }
                    else
                    {
                        res = -1;
                    }
                }
                else
                {
                    res = -1;
                }

                var result = new FunctionResult<object>();
                result.Arguments = arguments;
                result.Result = res;
                return result;
            }
            catch (Exception ex)
            {
                var result = new FunctionResult<object>();
                result.Arguments = arguments;
                result.Error = DefaultError.FromException(ex);
                return result;
            }
        }


        public class Descriptor : IFunctionDescriptor
        {
            public string Name => nameof(ToggleMuteAudioDeviceFunction);
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

            public IFunction<object> Instantiate()
            {
                return new ToggleMuteAudioDeviceFunction();
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
            _audioController.Dispose();
        }

    }
}