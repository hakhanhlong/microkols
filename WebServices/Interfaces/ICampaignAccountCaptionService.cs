using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;
namespace WebServices.Interfaces
{
    public interface ICampaignAccountCaptionService
    {
        Task<ListCampaignAccountCaptionViewModel> GetCampaignAccountCaptions(int campaignAccountId, string order, int page, int pagesize);
        Task<ListCampaignAccountCaptionViewModel> GetCampaignAccountCaptionsByCampaignId(int campaignId, string order, int page, int pagesize);
        Task<int> CreateCampaignAccountCaption(CreateCampaignAccountCaptionViewModel model, string username);
        Task<EditCampaignAccountCaptionViewModel> GetEditCampaignAccountCaption(int CampaignAccountCaptionId);
        Task<bool> EditCampaignAccountCaption(EditCampaignAccountCaptionViewModel model, string username);
        Task<bool> UpdateStatus(int id, CampaignAccountCaptionStatus status, string username);
    }
}
