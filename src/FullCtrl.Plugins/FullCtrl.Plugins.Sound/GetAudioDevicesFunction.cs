using System;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl.Base;

namespace FullCtrl.Plugins.Sound
{
    public class GetAudioDevicesFunction : IFunctionDescriptor, IFunction
    {
        public string Name => nameof(GetAudioDevicesFunction);
        public bool CanExecuteRemotely => true;

        public IParameterCollection GetParameters()
        {
            var res = new ParameterCollection();
            res["ApiAddress"] = new Parameter
            {
                Name = "ApiAddress",
                Required = true,
                Type = typeof(Uri),
                Value = new Uri("http://localhost:9000/api/v1/"),
            };
            res["DeviceType"] = new Parameter
            {
                Name = "DeviceType",
                Required = false,
                Type = typeof(AudioDeviceType?),
                Value = null,
            };
            res["DeviceState"] = new Parameter
            {
                Name = "DeviceState",
                Required = false,
                Type = typeof(AudioDeviceState?),
                Value = null,
            };
            return res;
        }

        public IFunction Instantiate()
        {
            return this;
        }
        
        public async Task<IFunctionResult> Execute(IFunctionArguments arguments)
        {
            try
            {
                var api = new AudioControllerAPI();
                api.BaseUri = (Uri)arguments.Parameters["ApiAddress"].Value;

                AudioDeviceType? deviceType = null;
                AudioDeviceState? deviceState = null;

                IParameter param;
                if (arguments.Parameters.TryGetValue("DeviceType", out param))
                    deviceType = (AudioDeviceType?) param.Value;
                if (arguments.Parameters.TryGetValue("DeviceState", out param))
                    deviceState = (AudioDeviceState?)param.Value;

                var response = await api.GetAudioDevices(deviceType, deviceState);
                if (response?.Error != null)
                    return new FunctionResult { Arguments = arguments, Error = response.Error };
                
                var result = new FunctionResult();
                result.Arguments = arguments;
                result.Result = response?.Result;
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

    }
}
