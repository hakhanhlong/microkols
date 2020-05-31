using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class VideoGallerySpecification: BaseSpecification<VideoGallery>
    {
        public VideoGallerySpecification(bool isActive) : base(m => m.IsActive == isActive)
        { }
    }
}
