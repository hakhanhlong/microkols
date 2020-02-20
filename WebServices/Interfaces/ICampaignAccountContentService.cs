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
        Task<ListCampaignAccountContentViewModel> GetCampaignAccountContents(int campaignAccountId, string order, int page, int pagesize);
        Task<ListCampaignAccountContentViewModel> GetCampaignAccountContentsByCampaignId(int campaignid, string order, int page, int pagesize);
        Task<int> CreateCampaignAccountContent(CreateCampaignAccountContentViewModel model, string username);
        Task<EditCampaignAccountContentViewModel> GetEditCampaignAccountContent(int CampaignAccountContentId);
        Task<bool> EditCampaignAccountContent(EditCampaignAccountContentViewModel model, string username);
        Task<bool> UpdateStatus(int id, CampaignAccountContentStatus status, string username);
    }
}
