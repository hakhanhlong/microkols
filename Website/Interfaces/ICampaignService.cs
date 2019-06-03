using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;

namespace Website.Interfaces
{
    public interface ICampaignService
    {
        Task<List<CampaignTypeChargeViewModel>> GetCampaignTypeCharges();
        Task<int> CreateCampaign(int agencyid, CreateCampaignViewModel model, string username);
        Task<CampaignDetailsViewModel> GetCampaignDetailsByAgency(int agencyid, int id);
        Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, CampaignType? type, string keyword, int page, int pagesize);

        Task<bool> RequestAccountJoinCampaign(int agencyid, int campaignid, int accountid, string username);
        Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id);
    }
}
