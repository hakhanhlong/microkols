using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IVideoGalleryRepository: IRepository<VideoGallery>, IAsyncRepository<VideoGallery>
    {

    }
}
