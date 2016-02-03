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
        [Route("api/v1/process/{pid}")]
        public IResponseBase<IProcessDto> Get([FromBody] int pid)
        {
            var result = ProcessHelper.GetProcess(pid);

            var response = CreateResponse<IProcessDto>(result);
            return response;
        }

        [HttpGet]
        [Route("api/v1/process")]
        public IResponseBase<IEnumerable<IProcessDto>> Get()
        {
            var result = ProcessHelper.GetProcesses();

            var response = CreateResponse<IEnumerable<IProcessDto>>(result);
            return response;
        }

        [HttpGet]
        [Route("api/v1/process/Search/{Name}")]
        public IResponseBase<IEnumerable<IProcessDto>> GetByName([FromBody] string name)
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
        public IResponseBase<StartProcessResponse> StartProcess([ModelBinder(typeof(CustomObjectModelBinder))] StartProcessRequest request)
        {
            var result = Worker.StartProcess(request);

            var response = CreateResponse(result);
            return response;
        }

        [HttpPost, HttpPut, HttpDelete]
        [Route("api/v1/process/kill")]
        [Route("api/v1/process/kill/{ProcessID}")]
        public IResponseBase<KillProcessResponse> KillProcess([ModelBinder(typeof(CustomObjectModelBinder))] KillProcessRequest request)
        {
            var result = Worker.KillProcess(request);

            var response = CreateResponse(result);
            return response;
        }

    }
}
