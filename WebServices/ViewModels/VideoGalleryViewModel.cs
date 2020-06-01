using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebServices.ViewModels
{
    public class VideoGalleryViewModel
    {
        public VideoGalleryViewModel() { }

        public VideoGalleryViewModel(VideoGallery v) {

            Id = v.Id;
            ImageURL = v.ImageURL;
            VideoEmbed = v.VideoEmbed;
            EmbedKey = v.EmbedKey;
            IsActive = v.IsActive;
            Order = v.Order;
            DateCreated = v.DateCreated;
            DateModified = v.DateModified;
            UserCreated = v.UserCreated;
            UserModified = v.UserModified;

        }

        public int Id { get; set; }


        
        [Display(Name = "Ảnh đại diện")]
        public string ImageURL { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "URL mã nhúng")]
        public string VideoEmbed { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Key mã nhúng")]
        public string EmbedKey { get; set; }


        [Display(Name = "Hoạt động/Không hoạt động")]
        public bool IsActive { get; set; }

        public int Order { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public IFormFile fileUpload { get; set; }



    }

    public class ListVideoGalleryViewModel
    {
        public List<VideoGalleryViewModel> VideoGalleries { get; set; }
        public PagerViewModel Pager { get; set; }
    }
}
