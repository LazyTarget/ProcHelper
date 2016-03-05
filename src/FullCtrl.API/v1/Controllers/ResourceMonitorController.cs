using System.Web.Http;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class ResourceMonitorController : BaseController
    {
        private readonly ResourceMonitor _resourceMonitor;

        public ResourceMonitorController()
        {
            _resourceMonitor = new ResourceMonitor();
        }
        

        [HttpGet, HttpPost, HttpPut]
        [Route("api/v1/resource/cpu/usage")]
        public IResponseBase<object> GetCurrentCpuUsage()
        {
            object result = _resourceMonitor.GetCurrentCpuUsage();

            var response = CreateResponse(result);
            return response;
        }

    }
}
