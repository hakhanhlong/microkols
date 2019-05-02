using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public partial class Banner : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public BannerPosition Position { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public int DisplayOrder { get; set; }
        public bool Deleted { get; set; }
        public bool Published { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }
    public enum BannerPosition
    {
        HomeIndex,

    }
    public static class BannerExt
    {
        public static string ToText(this BannerPosition position)
        {

            if (position == BannerPosition.HomeIndex)
            {
                return "Trang chủ";
            }

          
            return position.ToString();
        }
    }
}
