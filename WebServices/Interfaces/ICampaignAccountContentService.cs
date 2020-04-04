using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;
namespace WebServices.Interfaces
{
    public interface ICampaignAccountContentService
    {
        Task<bool> IsValidContent(int campaignAccountid);
        Task<ListCampaignAccountContentViewModel> GetCampaignAccountContents(int campaignAccountId, string order, int page, int pagesize);
        Task<ListGroupCampaignAccountContentViewModel> GetGroupCampaignAccountContentsByCampaignId(int campaignId, string order, int page, int pagesize);
        Task<int> CreateCampaignAccountContent(CreateCampaignAccountContentViewModel model, string username);
        Task<EditCampaignAccountContentViewModel> GetEditCampaignAccountContent(int CampaignAccountContentId);
        Task<bool> EditCampaignAccountContent(EditCampaignAccountContentViewModel model, string username);
        Task<bool> UpdateStatus(int id, CampaignAccountContentStatus status, string username);
        Task<bool> UpdateNote(int id, string note, string username);
    }
}
