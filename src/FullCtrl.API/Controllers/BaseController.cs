using System.Web.Http;
using FullCtrl.Base;

namespace FullCtrl.API.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected Worker Worker => new Worker();
        protected IProcessHelper ProcessHelper => new ProcessHelper();
        
        protected BaseController()
        {
            
        }

    }
}
