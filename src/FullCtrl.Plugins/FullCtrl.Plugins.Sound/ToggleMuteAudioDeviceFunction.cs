using System;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl.Base;

namespace FullCtrl.Plugins.Sound
{
    public class ToggleMuteAudioDeviceFunction : IFunctionDescriptor, IFunction
    {
        public string Name => nameof(ToggleMuteAudioDeviceFunction);
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
            res["DeviceID"] = new Parameter
            {
                Name = "DeviceID",
                Required = false,
                Type = typeof(string),
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

                string deviceID = null;
                IParameter deviceIDParam;
                if (arguments.Parameters.TryGetValue("DeviceID", out deviceIDParam))
                    deviceID = (string)deviceIDParam.Value;

                if (string.IsNullOrEmpty(deviceID))
                {
                    AudioDeviceType? deviceType = null;
                    AudioDeviceState? deviceState = null;
                    var response = await api.GetAudioDevices(deviceType, deviceState);
                    if (response?.Error != null)
                        return new FunctionResult {Arguments = arguments, Error = response.Error};

                    var defaultDevice = response?.Result?.FirstOrDefault(x => x.DefaultDevice);
                    if (defaultDevice != null)
                        deviceID = defaultDevice.ID.ToString();
                }

                var response2 = await api.ToggleDeviceMute(deviceID);
                if (response2?.Error != null)
                    return new FunctionResult { Arguments = arguments, Error = response2.Error };
                
                var result = new FunctionResult();
                result.Arguments = arguments;
                result.Result = response2?.Result;
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