using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FullCtrl.Base;
using FullCtrl.Plugins.Sound;

namespace FullCtrl.API.v1.Controllers
{
    public class AudioController : BaseController
    {
        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio/sessions")]
        [Route("api/v1/audio/sessions/list")]
        public async Task<IResponseBase<IEnumerable<AudioSession>>> GetSessions(string deviceID)
        {
            var function = new GetAudioSessionsFunction();
            var parameters = function.GetParameters();
            parameters.SetParamValue(GetAudioSessionsFunction.ParameterKeys.DeviceID, deviceID);

            var arguments = new FunctionArguments();
            arguments.Parameters = parameters;
            var functionResult = await ExecuteFunction(function, arguments);

            var response = CreateFunctionResponse<IEnumerable<AudioSession>>(functionResult);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio/devices")]
        [Route("api/v1/audio/devices/list")]
        public async Task<IResponseBase<IEnumerable<AudioDevice>>> GetDevices()
        {
            var response = await GetDevices(null);
            return response;
        }
        
        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio/devices/list/{deviceType}")]
        public async Task<IResponseBase<IEnumerable<AudioDevice>>> GetDevices(AudioDeviceType? deviceType)
        {
            var response = await GetDevices(deviceType, null);
            return response;
        }

        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio/devices/list/{deviceType}/{deviceState}")]
        public async Task<IResponseBase<IEnumerable<AudioDevice>>> GetDevices(AudioDeviceType? deviceType, AudioDeviceState? deviceState)
        {
            var function = new GetAudioDevicesFunction();
            var parameters = function.GetParameters();
            parameters.SetParamValue(GetAudioDevicesFunction.ParameterKeys.DeviceType, deviceType);
            parameters.SetParamValue(GetAudioDevicesFunction.ParameterKeys.DeviceState, deviceState);

            var arguments = new FunctionArguments();
            arguments.Parameters = parameters;
            var functionResult = await ExecuteFunction(function, arguments);

            var response = CreateFunctionResponse<IEnumerable<AudioDevice>>(functionResult);
            return response;
        }


        [HttpPost, HttpPut]
        [Route("api/v1/audio/device/set")]
        [Route("api/v1/audio/device/set/{deviceID}")]
        public async Task<IResponseBase<object>> SetDefaultDevice(string deviceID)
        {
            //object result = null;
            //Guid guid;
            //if (Guid.TryParse(deviceID, out guid))
            //{
            //    var device = _audioController.GetDevice(guid);
            //    if (device != null)
            //    {
            //        result = _audioController.SetDefaultDevice(device);
            //    }
            //    else
            //    {
            //        result = -1;
            //    }
            //}
            //else
            //{
            //    result = -1;
            //}

            //var response = CreateResponse(result);
            //return response;

            throw new NotImplementedException();
        }


        [HttpPost, HttpPut]
        [Route("api/v1/audio/device/togglemute")]
        [Route("api/v1/audio/device/togglemute/{deviceID}")]
        public async Task<IResponseBase<object>> ToggleDeviceMute(string deviceID)
        {
            var function = new ToggleMuteAudioDeviceFunction();
            var parameters = function.GetParameters();
            parameters.SetParamValue(ToggleMuteAudioDeviceFunction.ParameterKeys.DeviceID, deviceID);

            var arguments = new FunctionArguments();
            arguments.Parameters = parameters;
            var functionResult = await ExecuteFunction(function, arguments);

            var response = CreateFunctionResponse<object>(functionResult);
            return response;
        }
        

    }
}
