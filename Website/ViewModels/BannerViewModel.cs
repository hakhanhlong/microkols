using Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Website.ViewModels
{
    public class BannerViewModel
    {

        public BannerViewModel()
        {

        }

        public BannerViewModel(Banner banner)
        {

            Image = banner.Image;
            Description = banner.Description;

            Position = banner.Position;
            Title = banner.Title;
            Url = banner.Url;
        }
        public static List<BannerViewModel> GetList(IEnumerable<Banner> banners)
        {
          
            return banners.Select(m=> new BannerViewModel(m)).ToList();

        }



        public BannerPosition Position { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }
    }
}