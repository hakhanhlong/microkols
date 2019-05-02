using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface ISharedService
    {
        Task<List<BannerViewModel>> GetBanners(BannerPosition position);
        Task<List<CategoryViewModel>> GetCategories();
        Task<List<EntityViewModel>> GetCities();
        Task<EntityViewModel> GetCity(int id);
        Task<EntityViewModel> GetDistrict(int id);
        Task<List<EntityViewModel>> GetDistricts(int cityid);
    }
}