using System.Web.Http;
using System.Web.Http.ModelBinding;
using FullCtrl.API.Interfaces;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class MouseController : BaseController
    {
        [HttpGet]
        [Route("api/v1/mouse")]
        public IResponseBase<MouseInfo> GetInfo()
        {
            var result = Worker.GetMouseInfo();

            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/mouse/moveby")]
        [Route("api/v1/mouse/moveby/{X}/{Y}")]
        public IResponseBase<MouseInfo> MoveMouseBy([ModelBinder(typeof(CustomObjectModelBinder))] MoveMouseBy request)
        {
            var result = Worker.MoveMouseBy(request);

            var response = CreateResponse(result);
            return response;
        }


        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/mouse/moveto")]
        [Route("api/v1/mouse/moveto/{X}/{Y}")]
        public IResponseBase<MouseInfo> MoveMouseTo([ModelBinder(typeof(CustomObjectModelBinder))] MoveMouseTo request)
        {
            var result = Worker.MoveMouseTo(request);

            var response = CreateResponse(result);
            return response;
        }


        [Route("api/v1/mouse/movetovirtual")]
        [Route("api/v1/mouse/movetovirtual/{X}/{Y}")]
        public IResponseBase<MouseInfo> MoveMouseToPositionOnVirtualDesktop([ModelBinder(typeof(CustomObjectModelBinder))] MoveMouseToPositionOnVirtualDesktop request)
        {
            var result = Worker.MoveMouseToPositionOnVirtualDesktop(request);

            var response = CreateResponse(result);
            return response;
        }
    }
}
