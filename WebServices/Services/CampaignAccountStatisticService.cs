
using Common.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Hangfire;
using Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebServices.Services
{
    public class CampaignAccountStatisticService : BaseService, ICampaignAccountStatisticService
    {
        private readonly ICampaignAccountStatisticRepository _CampaignAccountStatisticRepository;
        public CampaignAccountStatisticService(ICampaignAccountStatisticRepository CampaignAccountStatisticRepository)
        {
            _CampaignAccountStatisticRepository = CampaignAccountStatisticRepository;
        }

        #region CampaignAccountStatistic
        public async Task<List<CampaignAccountStatisticViewModel>> GetCampaignAccountStatistics(int campaignAccountId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountStatisticByCampaignAccountIdSpecification(campaignAccountId);

            var list = await _CampaignAccountStatisticRepository.ListPagedAsync(filter, "Date_desc", page, pagesize);

            return CampaignAccountStatisticViewModel.GetList(list);
        }
        public async Task<List<CampaignAccountStatisticViewModel>> GetCampaignAccountStatisticsByCampaignId(int campaignId, string order, int page, int pagesize)
        {


            var filter = new CampaignAccountStatisticByCampaignIdSpecification(campaignId);

            var total = await _CampaignAccountStatisticRepository.CountAsync(filter);
            var list = await _CampaignAccountStatisticRepository.ListPagedAsync(filter, "Date_desc", page, pagesize);


            return CampaignAccountStatisticViewModel.GetList(list);
        }




        #endregion
    }
}
