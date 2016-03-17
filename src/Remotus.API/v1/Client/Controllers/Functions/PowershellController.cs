using System.Web.Http;
using Remotus.Base;

namespace Remotus.API.v1.Client.Controllers.Functions
{
    public class PowershellController : BaseController
    {
        [HttpPost, HttpPut]
        [Route("api/v1/powershell")]
        [Route("api/v1/powershell/file/{fileName}")]
        public IResponseBase<PowershellResponse> RunFile(string fileName, bool redirectStandardOutput = true)
        {
            var request = new PowershellFileRequest
            {
                FileName = fileName,
                RedirectStandardOutput = redirectStandardOutput,
            };
            var result = PowershellHelper.RunFile(request);

            var response = CreateResponse(result);
            return response;
        }


        [Route("api/v1/powershell")]
        [Route("api/v1/powershell/query/{query}")]
        public IResponseBase<PowershellResponse> RunQuery(string query, bool redirectStandardOutput = true)
        {
            var request = new PowershellQueryRequest
            {
                Query = query,
                RedirectStandardOutput = redirectStandardOutput,
            };
            var result = PowershellHelper.RunQuery(request);

            var response = CreateResponse(result);
            return response;
        }
    }
}
