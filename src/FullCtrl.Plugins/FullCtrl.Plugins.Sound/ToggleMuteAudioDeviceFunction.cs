using System;
using System.Linq;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using FullCtrl.Base;

namespace FullCtrl.Plugins.Sound
{
    public class ToggleMuteAudioDeviceFunction : IFunctionDescriptor, IFunction
    {
        private CoreAudioController _audioController = new CoreAudioController();

        public ToggleMuteAudioDeviceFunction()
        {
            
        }

        public string Name => nameof(ToggleMuteAudioDeviceFunction);
        public bool CanExecuteRemotely => true;

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
        
        public async Task<IFunctionResult> Execute(IExecutionContext context, IFunctionArguments arguments)
        {
            try
            {
                object res = null;
                var guid = arguments?.Parameters.GetParamValue<Guid>(ParameterKeys.DeviceID);
                if (guid.HasValue && guid != Guid.Empty)
                {
                    var device = _audioController.GetDevice(guid.Value);
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
            public const string DeviceID = "DeviceID";
        }

        public void Dispose()
        {
            _audioController.Dispose();
        }

    }
}