using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;
namespace WebServices.Interfaces
{
    public interface ICampaignAccountStatisticService
    {
        Task<List<CampaignAccountStatisticViewModel>> GetCampaignAccountStatistics(int campaignAccountId, string order, int page, int pagesize);
        Task<List<CampaignAccountStatisticViewModel>> GetCampaignAccountStatisticsByCampaignId(int campaignId, string order, int page, int pagesize);
    }
}
