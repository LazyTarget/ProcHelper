using System.Collections.Generic;

namespace Remotus.Web.Models
{
    public class MenuListViewModel
    {
        public ProfileItemViewModel Profile { get; set; }
        public IList<MenuListGroup> Groups { get; set; }
    }

    public class MenuListGroup
    {
        public IList<MenuListGroupItem> Items { get; set; }
        public string Text { get; set; }
    }

    public class MenuListGroupItem
    {
        public string Text { get; set; }
        public string Href { get; set; }
        public string ImgSrc { get; set; }
        public string Glyphicon { get; set; }
        public bool Disabled { get; set; }
        public bool Hidden { get; set; }
        public int BadgeCount { get; set; }
    }
}