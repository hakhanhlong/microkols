using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Common.Extensions
{
    public static class EnumExtensions
    {

       
        // This extension method is broken out so you can use a similar pattern with 
        // other MetaData elements in the future. This is your base method for each.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        // This method creates a specific call to the above method, requesting the
        // Description MetaData attribute.
        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static string ToDisplayName(this Enum value)
        {
            var attribute = value.GetAttribute<DisplayNameAttribute>();
            return attribute == null ? value.ToString() : attribute.DisplayName;
        }
    }
}
