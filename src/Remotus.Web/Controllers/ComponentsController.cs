using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FullCtrl.Web.Controllers
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