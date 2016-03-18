using System;

namespace Remotus.API
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ControllerCategoryAttribute : Attribute
    {
        public ControllerCategoryAttribute()
        {
        }

        public ControllerCategoryAttribute(string categoryName)
            : this()
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }
    }
}
