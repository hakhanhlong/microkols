using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface ICampaignService
    {
        Task<string> GetCampaignCode(int id);
        Task<bool> CreateCampaignAccount(int agencyid, int campaignid, int accountid, int amount, string username);
        Task UpdateCampaignAccountExpired(int campaignid = 0, int agencyid = 0);
        Task<List<CampaignTypeChargeViewModel>> GetCampaignTypeCharges();
        Task<CreateCampaignViewModel> GetCreateCampaign(int agencyid, CampaignType campaignType);
        Task<int> CreateCampaign(int agencyid, CreateCampaignViewModel model, string username);
        Task<CampaignDetailsViewModel> GetCampaignDetailsByAgency(int agencyid, int id);
        Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, CampaignType? type, CampaignStatus? status, string keyword, int page, int pagesize);
        Task<ListMarketPlaceViewModel> GetCampaignMarketPlaceByAccount(int accountid, string keyword, int page, int pagesize);
        Task<CampaignAccountByAccountViewModel> GetCampaignAccountByAccount(int accountid, int campaignid);
        Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, int accountid, string username);
        Task<bool> FeedbackJoinCampaignByAccount(int accountid, int campaignid, string username, bool confirmed);

        Task<bool> FeedbackJoinCampaignByAgency(int agencyid, int campaignid, int accountid, bool confirmed,  string username);
        Task<int> UpdateCampaignStatusByAgency(int agencyid, int campaignid, CampaignStatus status, string username);

        Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id);

        Task<ListCampaignWithAccountViewModel> GetListCampaignByAccount(int accountid,int type, string keyword, int page, int pagesize);
        Task<CampaignDetailsViewModel> GetCampaignDetailsByAccount(int accountid, int id);

        Task<bool> RequestJoinCampaignByAgency(int agencyid, int campaignid, string username);

        Task<int> UpdateCampaignAccountRef(int accountid, UpdateCampaignAccountRefViewModel model, string username);
        Task<int> SubmitCampaignAccountRefContent(int accountid, SubmitCampaignAccountRefContentViewModel model, string username);
        Task<int> SubmmitCampaignAccountChangeAvatar(int accountid, SubmmitCampaignAccountChangeAvatarViewModel model, string username);

        Task<MarketPlaceViewModel> GetCampaignMarketPlace(int id);
        Task<int> FeedbackCampaignAccountRefContent(int agencyid, int campaignid, int accountid, string username, int type, string newrefContent);

        Task<List<int>> GetEndedCampaignIds();
        Task<List<int>> GetFinishedAccountIdsByCampaignId(int campaignid);

        Task<bool> UpdateCampaignCompleted(int campaignid, string username);
        Task<bool> UpdateCampaignError(int campaignid, string note, string username);

        Task<bool> ReportCampaignAccount(int agencyid, ReportCampaignAccountViewModel model, string username);
        Task<bool> UpdateCampaignAccountRating(int agencyid, UpdateCampaignAccountRatingViewModel model, string username);

        Task<CampaignCounterViewModel> GetCampaignCounterByAccount(int accountid);
        Task<int> UpdateCampaignAccountRefImages(int accountid, UpdateCampaignAccountRefImagesViewModel model, string username);

        Task<CampaignViewModel> GetCampaignByRefId(int accountid, string facebookid);
        Task AutoUpdateStartedStatus(int campaignid);
        Task AutoUpdateEndedStatus(int campaignid);
    }
}
