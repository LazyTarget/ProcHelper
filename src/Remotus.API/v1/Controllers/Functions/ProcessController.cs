using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Remotus.Base;

namespace Remotus.API.v1.Controllers.Functions
{
    public class ProcessController : BaseController
    {
        [HttpGet]
        [Route("api/v1/process")]
        [Route("api/v1/process/{pid}")]
        [Route("api/v1/process/get/{pid}")]
        public IResponseBase<IProcessDto> Get(int pid)
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
        [Route("api/v1/process/list/{name}")]
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
        

        [HttpPost, HttpPut]
        [Route("api/v1/process/switchto")]
        [Route("api/v1/process/switchto/{processID}")]
        public IResponseBase<ProcessDto> SwitchToMainWindow(int processID)
        {
            var proc = ProcessHelper.GetProcess(processID);
            var result = ProcessHelper.SwitchToMainWindow(new IntPtr(proc.MainWindowHandle));

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
