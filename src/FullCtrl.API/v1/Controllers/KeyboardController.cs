using System.Threading;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FullCtrl.API.Interfaces;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class KeyboardController : BaseController
    {
        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/keyboard/key/isdown")]
        [Route("api/v1/keyboard/key/isdown/{VirtualKeyCode}")]
        public IResponseBase<KeyResponse> IsKeyDown([ModelBinder(typeof(CustomObjectModelBinder))] IsKeyDown request)
        {
            var result = Worker.IsKeyDown(request);

            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/keyboard/key/down")]
        [Route("api/v1/keyboard/key/down/{VirtualKeyCode}")]
        public IResponseBase<KeyResponse> KeyDown([ModelBinder(typeof(CustomObjectModelBinder))] KeyDownRequest request)
        {
            var result = Worker.KeyDown(request);

            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/keyboard/key/up")]
        [Route("api/v1/keyboard/key/up/{VirtualKeyCode}")]
        public IResponseBase<KeyResponse> KeyUp([ModelBinder(typeof(CustomObjectModelBinder))] KeyUpRequest request)
        {
            var result = Worker.KeyUp(request);

            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/keyboard/key/press")]
        [Route("api/v1/keyboard/key/press/{VirtualKeyCode}")]
        public IResponseBase<KeyResponse> KeyPress([ModelBinder(typeof(CustomObjectModelBinder))] KeyPressRequest request)
        {
            var result = Worker.KeyPress(request);

            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/keyboard/text/write")]
        [Route("api/v1/keyboard/text/write/{Text}")]
        public IResponseBase<object> WriteTextEntry(string text)
        {
            Thread.Sleep(2000);
            InputHelper.WriteTextEntry(text);

            var response = CreateResponse<object>(text);
            return response;
        }
    }
}
