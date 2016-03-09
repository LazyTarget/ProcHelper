using System;
using System.Collections.Generic;
using System.Linq;

namespace FullCtrl.Web.Models
{
    public class SideBarViewModel
    {
        public ProfileItemViewModel Profile { get; set; }
        public IList<SideBarGroup> Groups { get; set; }
    }

    public class SideBarGroup
    {
        public IList<SideBarItem> Items { get; set; }
        public string Text { get; set; }
    }

    public class SideBarItem
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string ImgSrc { get; set; }
        public bool Disabled { get; set; }
        public bool Hidden { get; set; }
    }
}