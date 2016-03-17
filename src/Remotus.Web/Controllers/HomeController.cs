using System.Web.Mvc;

namespace Remotus.Web.Controllers
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
