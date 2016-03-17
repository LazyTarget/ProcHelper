using System.Web.Mvc;

namespace Remotus.Web.Controllers
{
    public partial class ComponentsController : Controller
    {
        public ActionResult MenuList()
        {
            var model = GetMenuListViewModel();
            return PartialView("_MenuList", model);
        }
    }
}