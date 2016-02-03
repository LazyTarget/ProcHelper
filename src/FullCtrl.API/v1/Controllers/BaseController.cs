using System;
using System.Web.Http;
using FullCtrl.API.Interfaces;
using FullCtrl.API.v1.Models;
using FullCtrl.Base;

namespace FullCtrl.API.v1.Controllers
{
    public abstract class BaseController : ApiController, IBaseController
    {
        protected Worker Worker => new Worker();
        protected IProcessHelper ProcessHelper => new ProcessHelper();
        protected IPowershellHelper PowershellHelper => new PowershellHelper(ProcessHelper);
        protected IInputHelper InputHelper => new InputHelper();
        protected IWinServiceHelper WinServiceHelper => new WinServiceHelper();

        protected BaseController()
        {
            
        }


        public ResponseBase<TResult> CreateResponse<TResult>(TResult result = default(TResult))
        {
            var response = ResponseBase.Create<TResult>(Request, result);
            return response;
        }

    }
}
