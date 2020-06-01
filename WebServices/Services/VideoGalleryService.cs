using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;

using System.Linq;
using Core.Entities;

namespace WebServices.Services
{
    public class VideoGalleryService: IVideoGalleryService
    {

        IVideoGalleryRepository _IVideoGalleryRepository;
        public VideoGalleryService(IVideoGalleryRepository __IVideoGalleryRepository)
        {
            _IVideoGalleryRepository = __IVideoGalleryRepository;
        }

        public async Task<ListVideoGalleryViewModel> GetByType(bool isActive, int pageindex)
        {
            var filter = new VideoGallerySpecification(isActive);
            var list = await _IVideoGalleryRepository.ListPagedAsync(filter, "", pageindex, 25);
            var count = await _IVideoGalleryRepository.CountAsync(filter);

            return new ListVideoGalleryViewModel()
            {
                VideoGalleries = list.Select(v => new VideoGalleryViewModel(v)).ToList(),
                Pager = new PagerViewModel()
                {
                    Page = pageindex,
                    PageSize = 25,
                    Total = count
                }
            };
        }

        public async Task<ListVideoGalleryViewModel> GetAll(int pageindex)
        {
            var filter = new VideoGallerySpecification();
            var list = await _IVideoGalleryRepository.ListPagedAsync(filter, "", pageindex, 25);
            var count = await _IVideoGalleryRepository.CountAsync(filter);

            return new ListVideoGalleryViewModel()
            {
                VideoGalleries = list.Select(v => new VideoGalleryViewModel(v)).ToList(),
                Pager = new PagerViewModel()
                {
                    Page = pageindex,
                    PageSize = 25,
                    Total = count
                }
            };
        }

        public async Task<int> Create(VideoGalleryViewModel model)
        {
            try
            {
                var list = _IVideoGalleryRepository.ListAll().Where(o => o.IsActive == model.IsActive).Select(q => q.Order).ToList();
                int maxOrder = 0;
                if (list.Count > 0)
                {
                    maxOrder = list.Max() + 1;
                }
                else
                {
                    maxOrder = 1;
                }

                VideoGallery vgallery = new VideoGallery()
                {
                    Order = maxOrder,
                    IsActive = model.IsActive,                    
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,                    
                    UserCreated = model.UserCreated,
                    UserModified = model.UserModified,
                    EmbedKey = model.EmbedKey,
                    ImageURL = model.ImageURL,
                    VideoEmbed = model.VideoEmbed                   

                };

                return (await _IVideoGalleryRepository.AddAsync(vgallery)).Id;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<int> Update(VideoGalleryViewModel model)
        {
            try
            {
                var vgallery = await _IVideoGalleryRepository.GetByIdAsync(model.Id);
                if (vgallery != null)
                {                    
                    vgallery.IsActive = model.IsActive;
                    vgallery.DateModified = DateTime.Now;
                    vgallery.UserModified = model.UserModified;
                    vgallery.EmbedKey = model.EmbedKey;
                    vgallery.ImageURL = model.ImageURL;
                    vgallery.VideoEmbed = model.VideoEmbed;

                    await _IVideoGalleryRepository.UpdateAsync(vgallery);

                    return vgallery.Id;
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }
    }
}
