using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using FullCtrl.API.Interfaces;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public class ProcessController : BaseController
    {
        [HttpGet]
        [Route("api/v1/process")]
        [Route("api/v1/process/{pid}")]
        [Route("api/v1/process/get/{pid}")]
        public IResponseBase<IProcessDto> Get([FromBody] int pid)
        {
            var result = ProcessHelper.GetProcess(pid);

            var response = CreateResponse<IProcessDto>(result);
            return response;
        }


        [HttpGet]
        [Route("api/v1/process")]
        [Route("api/v1/process/list")]
        public IResponseBase<IEnumerable<IProcessDto>> List()
        {
            var result = ProcessHelper.GetProcesses();

            var response = CreateResponse<IEnumerable<IProcessDto>>(result);
            return response;
        }


        [HttpGet]
        [Route("api/v1/process/list")]
        [Route("api/v1/process/list/{Name}")]
        public IResponseBase<IEnumerable<IProcessDto>> ListByName([FromBody] string name)
        {
            var result = ProcessHelper.GetProcessesByName(name);

            var response = CreateResponse<IEnumerable<IProcessDto>>(result);
            return response;
        }
        

        [HttpPost, HttpPut]
        [Route("api/v1/process/start")]
        [Route("api/v1/process/start/{FileName}")]
        [Route("api/v1/process/start/{FileName}/{Arguments}")]
        [Route("api/v1/process/start/{FileName}/{Arguments}/{WorkingDirectory}")]
        public IResponseBase<StartProcessResponse> Start([ModelBinder(typeof(CustomObjectModelBinder))] StartProcessRequest request)
        {
            var result = Worker.StartProcess(request);

            var response = CreateResponse(result);
            return response;
        }


        [HttpPost, HttpPut, HttpDelete]
        [Route("api/v1/process/kill")]
        [Route("api/v1/process/kill/{ProcessID}")]
        public IResponseBase<KillProcessResponse> Kill([ModelBinder(typeof(CustomObjectModelBinder))] KillProcessRequest request)
        {
            var result = Worker.KillProcess(request);

            var response = CreateResponse(result);
            return response;
        }

    }
}
