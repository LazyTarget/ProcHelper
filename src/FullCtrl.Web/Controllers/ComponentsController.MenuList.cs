using System.Collections.Generic;
using System.Linq;
using FullCtrl.Web.Models;

namespace FullCtrl.Web.Controllers
{
    public partial class ComponentsController
    {
        protected MenuListViewModel GetMenuListViewModel()
        {
            var profileItems = new List<MenuListGroupItem>();
            var favoriteItems = new List<MenuListGroupItem>();
            var functionItems = new List<MenuListGroupItem>();
            var deviceItems = new List<MenuListGroupItem>();
            var selfItems = new List<MenuListGroupItem>();


            #region Profile

            profileItems.Add(new MenuListGroupItem
            {
                Text = "Profile",
                Href = Url.Action("Index", "Me"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            profileItems.Add(new MenuListGroupItem
            {
                Text = "Notifications",
                Href = Url.Action("Notifications", "Me"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });

            #endregion


            #region Favorites
            


            #endregion


            #region Functions

            functionItems.Add(new MenuListGroupItem
            {
                Text = "Execute (Process/Script)",
                Href = Url.Action("Execute", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });

            #endregion


            #region Device

            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Processes",
                Href = Url.Action("Process", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Services",
                Href = Url.Action("Windows", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Windows (Alt-Tab)",
                Href = Url.Action("Windows", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Mouse",
                Href = Url.Action("Mouse", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Keyboard",
                Href = Url.Action("Keyboard", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Input",
                Href = Url.Action("Input", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Volume",
                Href = Url.Action("Volume", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });

            #endregion


            #region Self

            selfItems.Add(new MenuListGroupItem
            {
                Text = "Logs",
                Href = Url.Action("Log", "Function"),
                Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            selfItems.Add(new MenuListGroupItem
            {
                Text = "Settings",
                Href = Url.Action("Settings", "Function"),
                Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            selfItems.Add(new MenuListGroupItem
            {
                Text = "Login",
                Href = Url.Action("Login", "Account"),
                Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });
            selfItems.Add(new MenuListGroupItem
            {
                Text = "Logout",
                Href = Url.Action("LogOff", "Account"),
                Hidden = (User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "",
            });

            #endregion




            var groups = new List<MenuListGroup>
            {
                new MenuListGroup
                {
                    //Text = "Profile",
                    Items = profileItems,
                },

                new MenuListGroup
                {
                    Text = "Favorites",
                    Items = favoriteItems,
                },

                new MenuListGroup
                {
                    Text = "Functions",
                    Items = functionItems,
                },

                new MenuListGroup
                {
                    Text = "Device",
                    Items = deviceItems,
                },

                new MenuListGroup
                {
                    Text = "Info & Settings",
                    Items = selfItems,
                },
            };

            var model = new MenuListViewModel
            {
                Profile = new ProfileItemViewModel
                {
                    Name = User?.Identity?.Name,
                },
                Groups = groups,
            };
            return model;
        }
        
    }
}