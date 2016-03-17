using System.Web.Http;
using Remotus.Base;

namespace Remotus.API.v1.Client.Controllers.Functions
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
