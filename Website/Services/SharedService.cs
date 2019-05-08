using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Services
{
    public class SharedService : BaseService, ISharedService
    {
       
        private readonly IAsyncRepository<City> _cityRepository;
        private readonly IAsyncRepository<District> _districtRepository;
        private readonly IAsyncRepository<Banner> _bannerRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;

        private readonly ISettingRepository _settingRepository;

        public SharedService(
            IAsyncRepository<Category> categoryRepository,
            IAsyncRepository<Banner> bannerRepository,
             IAsyncRepository<City> cityRepository,
             IAsyncRepository<District> districtRepository,
             ISettingRepository settingRepository)
        {
            
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _categoryRepository = categoryRepository;
            _bannerRepository = bannerRepository;
            _settingRepository = settingRepository;

        }


        #region Category

        public async Task<List<CategoryViewModel>> GetCategories()
        {
            var banner = await _categoryRepository.ListAsync(new CategoryPublishedSpecification());
            return CategoryViewModel.GetList(banner);
        }
        #endregion

        #region banner

        public async Task<List<BannerViewModel>> GetBanners(BannerPosition position)
        {

            var banner = await _bannerRepository.ListAsync(new BannerPublishedSpecification(position));
            return BannerViewModel.GetList(banner.OrderBy(m => m.DisplayOrder));
        }
        #endregion


        #region City & District
        public async Task<List<EntityViewModel>> GetCities()
        {
            var citites = await _cityRepository.ListAllAsync();

            return EntityViewModel.GetList(citites);
        }

        public async Task<EntityViewModel> GetCity(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);
            if (city == null) return null;
            return new EntityViewModel(city);
        }

        public async Task<EntityViewModel> GetDistrict(int id)
        {
            var district = await _districtRepository.GetByIdAsync(id);
            if (district == null) return null;
            return new EntityViewModel(district);
        }

        public async Task<List<EntityViewModel>> GetDistricts(int cityid)
        {
            var filter = new DistrictSpecification(cityid);
            var districts = await _districtRepository.ListAsync(filter);

            return EntityViewModel.GetList(districts);
        }

        public async Task<SettingModel> GetSetting()
        {
            return await _settingRepository.GetSetting();
        }

        #endregion

    }
}
