using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using FullCtrl.Base;

namespace FullCtrl.API.Controllers
{
    public class ProcessController : BaseController
    {
        [Route("api/process")]
        [Route("api/process/{Name}")]
        public IEnumerable<ProcessDto> Get(string name)
        {
            var result = ProcessHelper.GetProcessesByName(name);
            return result;
        }
        
        [Route("api/process/{pid}")]
        public ProcessDto Get(int pid)
        {
            var result = ProcessHelper.GetProcess(pid);
            return result;
        }
        
        [HttpPost, HttpPut]
        [Route("api/process/start")]
        [Route("api/process/start/{FileName}")]
        [Route("api/process/start/{FileName}/{Arguments}")]
        [Route("api/process/start/{FileName}/{Arguments}/{WorkingDirectory}")]
        public StartProcessResponse Post(StartProcessRequest request)
        {
            var response = Worker.StartProcess(request);
            return response;
        }

        //[HttpPost, HttpPut, HttpDelete]
        [Route("api/process/kill")]
        [Route("api/process/kill/{ProcessID}")]
        public KillProcessResponse Delete([ModelBinder(typeof(CustomObjectModelBinder))] KillProcessRequest request)
        {
            var response = Worker.KillProcess(request);
            return response;
        }

    }
}
