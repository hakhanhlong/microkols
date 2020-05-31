using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class VideoGalleryRepository: EfRepository<VideoGallery>, IVideoGalleryRepository
    {
        public VideoGalleryRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
