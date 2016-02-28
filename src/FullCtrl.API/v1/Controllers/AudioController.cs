using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using CoreAudioApi;
using CoreAudioExtended;
using CoreAudioServer;
using FullCtrl.Base;
using Newtonsoft.Json;

namespace FullCtrl.API.v1.Controllers
{
    public class AudioController : BaseController
    {
        private static SessionManager _sessionManager;

        static AudioController()
        {
            _sessionManager = new SessionManager();
            _sessionManager.Start();
        }

        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio/sessions")]
        [Route("api/v1/audio/sessions/list")]
        public IResponseBase<IEnumerable<AudioSession>> GetSessions([ModelBinder(typeof(CustomObjectModelBinder))] object request)
        {
            List<EasyAudioSession> sessions;
            sessions = _sessionManager?.GetSessionList();

            //var res = sessions.Select(x => new AudioSession(x.Session));
            var res = sessions?.Select(FromAudioControllerState);
            IEnumerable<AudioSession> result = res?.ToList();
            
            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio/device/list")]
        [Route("api/v1/audio/devices")]
        [Route("api/v1/audio/devices/list")]
        public IResponseBase<IEnumerable<AudioDevice>> GetDevices()
        {
            var stateFilter = EDeviceState.DEVICE_STATEMASK_ALL;
            stateFilter = EDeviceState.DEVICE_STATE_ACTIVE;

            var devices = _sessionManager?._devEnum?.EnumerateAudioEndPoints(EDataFlow.eRender, stateFilter);
            IList<AudioDevice> res = devices?.Select(FromAudioDevice).ToList();
            if (res != null)
            {
                foreach (var audioDevice in res)
                {
                    if (audioDevice.ID == _sessionManager?._defaultDevice?.ID)
                        audioDevice.DefaultDevice = true;
                }
            }

            IEnumerable<AudioDevice> result = res;
            var response = CreateResponse(result);
            return response;
        }


        [HttpPost, HttpPut]
        [Route("api/v1/audio/device/set")]
        [Route("api/v1/audio/device/set/{deviceID}")]
        public IResponseBase<object> SetDefaultDevice(string deviceID)
        {
            var r = _sessionManager?.OnDefaultDeviceChanged(EDataFlow.eRender, ERole.eMultimedia, deviceID);

            object result = null;
            var response = CreateResponse(result);
            return response;
        }






        private AudioSession FromAudioControllerState(IAudioControllerState state)
        {
            var result = new AudioSession
            {
                ID = state.ID,
                Name = state.Name,
                Icon = state.Icon,
                GroupingParam = state.GroupingParam,
                Muted = state.Muted,
                Volume = state.Volume,
            };
            return result;
        }

        private AudioDevice FromAudioDevice(MMDevice state)
        {
            var result = new AudioDevice
            {
                ID = state.ID,
                FriendlyName = state.FriendlyName,
                Muted = state.AudioEndpointVolume.Mute,
                Volume = state.AudioEndpointVolume.MasterVolumeLevel,
                State = state.State,
                DefaultDevice = false,
            };
            return result;
        }

    }
}
