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
        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/audio")]
        public IResponseBase<IEnumerable<AudioSession>> Test([ModelBinder(typeof(CustomObjectModelBinder))] object request)
        {
            var sessionManager = new SessionManager();
            sessionManager.Start();

            List<EasyAudioSession> sessions;
            sessions = sessionManager.GetSessionList();

            while (sessions == null || !sessions.Any())
            {
                Thread.Sleep(250);
                sessions = sessionManager.GetSessionList();
            }

            //var res = sessions.Select(x => new AudioSession(x.Session));
            var res = sessions.Select(FromAudioControllerState);
            IEnumerable<AudioSession> result = res.ToList();

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

    }
}
