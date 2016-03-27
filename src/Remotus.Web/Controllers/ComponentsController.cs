using System.Threading.Tasks;
using System.Web.Mvc;
using Lux.Extensions;

namespace Remotus.Web.Controllers
{
    public partial class ComponentsController : Controller
    {
        public ActionResult MenuList()
        {
            //var model = GetMenuListViewModel_FromAPI().WaitForResult();

            //var task = GetMenuListViewModel_FromAPI();
            //task.ConfigureAwait(false);
            ////task.RunSynchronously();

            var task = Task.Run(GetMenuListViewModel_FromAPI);
            var model = task.Result;
            return PartialView("_MenuList", model);
        }
    }
}