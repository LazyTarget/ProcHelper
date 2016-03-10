using System;
using System.Linq;
using System.Threading.Tasks;
using FullCtrl.Base;
using FullCtrl.Internal;

namespace FullCtrl.Plugins.Sound
{
    public class GetAudioSessionsFunction : IFunctionDescriptor, IFunction
    {
        public string Name => nameof(GetAudioSessionsFunction);
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
                IParameter param;
                if (arguments.Parameters.TryGetValue("DeviceID", out param))
                    deviceID = (string)param.Value;

                var response = await api.GetAudioSessions(deviceID);
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
