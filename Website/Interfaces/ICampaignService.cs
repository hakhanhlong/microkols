using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface ICampaignService
    {
        Task<List<CampaignTypeViewModel>> GetCampaignTypes();
        Task<int> CreateCampaign(int agencyid, CreateCampaignViewModel model, string username);
        Task<CampaignDetailsViewModel> GetCampaignDetailsByAgency(int agencyid, int id);
        Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, int? campaignTypeId, string keyword, int page, int pagesize);
    }
}
