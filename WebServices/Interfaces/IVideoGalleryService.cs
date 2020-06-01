using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface IVideoGalleryService
    {
        Task<ListVideoGalleryViewModel> GetByType(bool isActive, int pageindex);

        Task<ListVideoGalleryViewModel> GetAll(int pageindex);

        Task<int> Create(VideoGalleryViewModel model);

        Task<int> Update(VideoGalleryViewModel model);



    }
}
