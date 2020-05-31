using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class VideoGallery : BaseEntityWithDate
    {
        public string ImageURL { get; set; }
        public string VideoEmbed { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
    }
}
