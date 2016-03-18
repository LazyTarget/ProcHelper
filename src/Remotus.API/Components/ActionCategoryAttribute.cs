using System;

namespace Remotus.API
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ActionCategoryAttribute : Attribute
    {
        public ActionCategoryAttribute()
        {
        }

        public ActionCategoryAttribute(string categoryName)
            : this()
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; set; }
    }
}
