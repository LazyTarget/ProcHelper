using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FullCtrl.Web.Controllers
{
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            return View("Dashboard");
        }
    }
}