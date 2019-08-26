using BackOffice.Models;
using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface ISharedBusiness
    {
        Task<List<BannerViewModel>> GetBanners(BannerPosition position);
        Task<List<CategoryViewModel>> GetCategories();
        Task<List<EntityViewModel>> GetCities();
        Task<EntityViewModel> GetCity(int id);
        Task<EntityViewModel> GetDistrict(int id);
        Task<List<EntityViewModel>> GetDistricts(int cityid);
        Task<SettingModel> GetSetting();
    }
}
