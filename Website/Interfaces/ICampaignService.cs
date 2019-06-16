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
        Task<CampaignAccountViewModel> GetCampaignAccountByAccount(int accountid, int campaignid);
        Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, int accountid, string username);
        Task<bool> ConfirmJoinCampaignByAccount(int accountid, int campaignid, string username);

        
        Task<int> UpdateCampaignStatusByAgency(int agencyid, int campaignid, CampaignStatus status, string username);

        Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id);

        Task<ListCampaignWithAccountViewModel> GetListCampaignByAccount(int accountid, string keyword, int page, int pagesize);
        Task<CampaignDetailsViewModel> GetCampaignDetailsByAccount(int accountid, int id);


        Task<int> UpdateCampaignAccountRef(int accountid, UpdateCampaignAccountRefViewModel model, string username);
        Task<int> SubmitCampaignAccountRefContent(int accountid, SubmitCampaignAccountRefContentViewModel model, string username);
        Task<int> SubmmitCampaignAccountChangeAvatar(int accountid, SubmmitCampaignAccountChangeAvatarViewModel model, string username);

        Task<int> FeedbackCampaignAccountRefContent(int agencyid, int campaignid, int accountid, string username, int type);

        Task<List<int>> GetEndedCampaignIds();
        Task<List<int>> GetFinishedAccountIdsByCampaignId(int campaignid);

        Task<bool> UpdateCampaignCompleted(int campaignid, string username);
        Task<bool> UpdateCampaignError(int campaignid, string note, string username);
    }
}
