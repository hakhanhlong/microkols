using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Extensions
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

        //// This method creates a specific call to the above method, requesting the
        //// Description MetaData attribute.
        //public static string ToDescription(this Enum value)
        //{
        //    var attribute = value.GetAttribute<DescriptionAttribute>();
        //    return attribute == null ? value.ToString() : attribute.Description;
        //}

        public static string ToShowName(this Enum value)
        {
            try
            {

                var attribute = value.GetAttribute<DisplayAttribute>();
                return attribute == null ? value.ToString() : attribute.Name;
            }
            catch
            {
                return value.ToString();
            }
        }

        public static string ToName(this Enum value)
        {
            try
            {

                var attribute = value.GetAttribute<DisplayAttribute>();
                return attribute == null ? value.ToString() : attribute.Name;
            }
            catch
            {
                return value.ToString();
            }
        }

        //public static string ToPriceText(this int price, string currency = "đ")
        //{
        //    return $"{price.ToString("n0")} {currency}";
        //}
        //public static string ToPriceText(this long price, string currency = "đ")
        //{
        //    return $"{price.ToString("n0")} {currency}";
        //}
    }
}
