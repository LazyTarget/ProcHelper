using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FullCtrl.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Chat()
        {
            return View();
        }
    }
}
