
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Services
{
    public class CampaignService : BaseService, ICampaignService
    {

        private readonly ICampaignRepository _campaignRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IAsyncRepository<CampaignType> _campaignTypeRepository;
        private readonly IAsyncRepository<CampaignOption> _campaignOptionRepository;
        private readonly ISettingRepository _settingRepository;
        private readonly ITransactionRepository _transactionRepository;

        public CampaignService(ICampaignRepository campaignRepository,
            ITransactionRepository transactionRepository,
            IWalletRepository walletRepository,
            IAsyncRepository<CampaignType> campaignTypeRepository,
            IAsyncRepository<CampaignOption> campaignOptionRepository,
             ISettingRepository settingRepository)
        {
            _campaignTypeRepository = campaignTypeRepository;
            _campaignOptionRepository = campaignOptionRepository;
            _campaignRepository = campaignRepository;
            _walletRepository = walletRepository;
            _settingRepository = settingRepository;
            _transactionRepository = transactionRepository;
        }

        #region Campaign

        public async Task<List<CampaignTypeViewModel>> GetCampaignTypes()
        {
            var filter = new CampaignTypePublishedSpecification();
            var types = await _campaignTypeRepository.ListAsync(filter);
            return CampaignTypeViewModel.GetList(types);
        }

        public async Task<ListCampaignViewModel> GetCampaignsByAgency(int agencyid,int? campaignTypeId ,string keyword, int page, int pagesize)
        {
            var filter = new CampaignByAgencySpecification(agencyid, campaignTypeId, keyword);
            var campaigns = await _campaignRepository.ListPagedAsync(filter, "DateModified_desc", page, pagesize);
            var total = await _campaignRepository.CountAsync(filter);

            return new ListCampaignViewModel()
            {
                Campaigns = CampaignViewModel.GetList(campaigns),
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }

        public async Task<CampaignDetailsViewModel> GetCampaignDetailsByAgency(int agencyid, int id)
        {
            var filter = new CampaignByAgencySpecification(agencyid,id);

            var campaign = await _campaignRepository.GetSingleBySpecAsync(filter);
            if (campaign != null)
            {
                var transactions = await _transactionRepository.ListAsync(new TransactionByCampaignSpecification(campaign.Id));
                return new CampaignDetailsViewModel(campaign, transactions);
            }
            return null;
        }



        public async Task<int> CreateCampaign(int agencyid, CreateCampaignViewModel model, string username)
        {
            var campaignType = await _campaignTypeRepository.GetByIdAsync(model.CampaignTypeId);
            if (campaignType == null)
            {
                return -1;
            }
            var settings = await _settingRepository.GetSetting();
            var wallet = await _walletRepository.GetBalance(EntityType.Agency, agencyid);
            var campaign = model.GetEntity(agencyid, campaignType, settings, username);
            if (campaign == null)
            {
                return -1;
            }

            await _campaignRepository.AddAsync(campaign);
            if (campaign.Id > 0)
            {
                if (model.CityId.HasValue)
                {
                    await _campaignOptionRepository.AddAsync(new CampaignOption()
                    {
                        CampaignId = campaign.Id,
                        Name = CampaignOptionName.City,
                        Value = model.CityId.Value.ToString()
                    });
                }
                if (model.Gender.HasValue)
                {
                    await _campaignOptionRepository.AddAsync(new CampaignOption()
                    {
                        CampaignId = campaign.Id,
                        Name = CampaignOptionName.Gender,
                        Value = model.Gender.Value.ToString()
                    });
                }

                if (model.AgeEnd.HasValue && model.AgeStart.HasValue)
                {
                    await _campaignOptionRepository.AddAsync(new CampaignOption()
                    {
                        CampaignId = campaign.Id,
                        Name = CampaignOptionName.AgeRange,
                        Value = $"{model.AgeStart.Value}-{model.AgeEnd.Value}"
                    });
                }

                if (model.CategoryId != null && model.CategoryId.Count > 0)
                {
                    foreach (var categoryid in model.CategoryId)
                    {
                        await _campaignOptionRepository.AddAsync(new CampaignOption()
                        {
                            CampaignId = campaign.Id,
                            Name = CampaignOptionName.Category,
                            Value = categoryid.ToString()
                        });
                    }

                }
                return campaign.Id;

            }



            return -1;
        }

        #endregion

    }
}
