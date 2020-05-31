using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebServices.ViewModels
{
    public class VideoGalleryViewModel
    {
        public VideoGalleryViewModel() { }

        public int Id { get; set; }

        public string ImageURL { get; set; }

        public string VideoEmbed { get; set; }

        public string EmbedKey { get; set; }


        [Display(Name = "Hoạt động/Không hoạt động")]
        public bool IsActive { get; set; }

        public int Order { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

    }

    public class ListVideoGalleryViewModel
    {
        public List<VideoGalleryViewModel> VideoGalleries { get; set; }
        public PagerViewModel Pager { get; set; }
    }
}
