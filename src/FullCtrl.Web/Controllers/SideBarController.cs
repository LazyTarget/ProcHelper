using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FullCtrl.Web.Models;

namespace FullCtrl.Web.Controllers
{
    public class SideBarController : Controller
    {
        public ActionResult MenuSideBar()
        {
            var groups = new List<SideBarGroup>
            {
                new SideBarGroup
                {
                    Text = "Account",
                    Items = new List<SideBarItem>
                    {
                        new SideBarItem
                        {
                            Text = "Login",
                            Href = Url.Action("Login", "Account"),
                            Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                        },
                        new SideBarItem
                        {
                            Text = "Logout",
                            Href = Url.Action("LogOff", "Account"),
                            Hidden = (User?.Identity?.IsAuthenticated ?? false),
                        },
                    },
                },
            };

            var model = new SideBarViewModel
            {
                Profile = new ProfileItemViewModel
                {
                    Name = User?.Identity?.Name,
                },
                Groups = groups,
            };
            return PartialView("_SideBar", model);
        }
    }
}