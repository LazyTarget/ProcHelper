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
        public IResponseBase<object> Test([ModelBinder(typeof(CustomObjectModelBinder))] object request)
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

            var res = sessions.Select(x => new AudioSession(x.Session));
            object result = res.ToList();

            var response = CreateResponse(result);
            return response;
        }


        private class AudioSession : EasyAudioSession
        {
            private string _iconBase64 = null;

            public AudioSession(AudioSessionControl session) : base(session)
            {
            }

            //[JsonIgnore]
            public override string ID { get { return base.ID; } }

            [JsonIgnore]
            public override Process SessionProcess { get { return base.SessionProcess; } }


            public string IconBase64
            {
                get
                {
                    if (_iconBase64 == null)
                    {
                        using (var memStream = new MemoryStream())
                        {
                            Icon.Save(memStream, ImageFormat.Jpeg);
                            var imgData = memStream.ToArray();
                            _iconBase64 = Convert.ToBase64String(imgData);
                        }
                    }
                    return _iconBase64;
                }
            }

        }
    }
}
