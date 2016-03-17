using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Remotus.Web.Models;

namespace Remotus.Web.Controllers.Api
{
    [Authorize]
    public class MeApiController : ApiController
    {
        private ApplicationUserManager _userManager;

        public MeApiController()
        {
        }

        public MeApiController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/Me
        public GetViewModel Get()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return new GetViewModel() { Hometown = user.Hometown };
        }
    }
}