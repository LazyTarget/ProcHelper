using System.Collections.Generic;
using Remotus.Web.Models;

namespace Remotus.Web.Controllers
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
                Href = Url.Action("Index", "Profile"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-user",
            });
            profileItems.Add(new MenuListGroupItem
            {
                Text = "Notifications",     // User notifications (triggered functions?)
                Href = Url.Action("Notifications", "Profile"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-bell",
                //Glyphicon = "glyphicon-inbox",
                BadgeCount = 3,
            });

            #endregion


            #region Favorites

            functionItems.Add(new MenuListGroupItem
            {
                Text = "Shutdown",
                Href = Url.Action("Shutdown", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-off",
            });

            #endregion


            #region Functions

            functionItems.Add(new MenuListGroupItem
            {
                Text = "Execute (Process/Script)",
                Href = Url.Action("Execute", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-console",
            });

            #endregion


            #region Device

            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Processes",
                Href = Url.Action("Plugin", "Function", new { pluginID = "8315E347-633E-4990-AF12-C0FFC4527485" }),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-tasks",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Services",
                Href = Url.Action("Services", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-cog",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Windows (Alt-Tab)",
                Href = Url.Action("Windows", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-list-alt",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Windows (Resolution / Default screen)",
                Href = Url.Action("Windows", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-resize-full",
            });
            //deviceItems.Add(new MenuListGroupItem
            //{
            //    Text = "Sound",
            //    Href = Url.Action("Sound", "Function"),
            //    Disabled = !(User?.Identity?.IsAuthenticated ?? false),
            //    Glyphicon = "glyphicon-volume-up",
            //});
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Sound",
                Href = Url.Action("Plugin", "Function", new { pluginID = "ABA6417A-65A2-4761-9B01-AA9DFFC074C0" }),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-volume-up",
            });

            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Input",
                Href = Url.Action("Input", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-italic",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Mouse",
                Href = Url.Action("Mouse", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-baby-formula",
            });
            deviceItems.Add(new MenuListGroupItem
            {
                Text = "Keyboard",
                Href = Url.Action("Keyboard", "Function"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-font",
            });

            #endregion


            #region Self

            selfItems.Add(new MenuListGroupItem
            {
                Text = "Events",    // Events that have occured (ex: errors/logs/executed functions)
                Href = Url.Action("Log", "Me"),
                Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-flash",
            });
            profileItems.Add(new MenuListGroupItem
            {
                Text = "News",      // News about FullCtrl
                Href = Url.Action("News", "Me"),
                Disabled = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-bullhorn",
                //Glyphicon = "glyphicon-inbox",
                BadgeCount = 3,
            });
            selfItems.Add(new MenuListGroupItem
            {
                Text = "Settings",  // Settings for FullCtrl
                Href = Url.Action("Settings", "Me"),
                Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-wrench",
            });
            selfItems.Add(new MenuListGroupItem
            {
                Text = "Login",
                Href = Url.Action("Login", "Account"),
                Hidden = !(User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-log-in",
            });
            selfItems.Add(new MenuListGroupItem
            {
                Text = "Logout",
                Href = Url.Action("LogOff", "Account"),
                Hidden = (User?.Identity?.IsAuthenticated ?? false),
                Glyphicon = "glyphicon-log-out",
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