using BackOffice.Models;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface ICampaignBusiness
    {
        ListCampaignViewModel GetListCampaign(int pageindex, int pagesize);

        Task<ListCampaignViewModel> GetCampaignByStatus(CampaignStatus? status, int pageindex, int pagesize);

        Task<ListCampaignViewModel> GetListCampaignByAgency(int agencyid, int pageindex, int pagesize);

        Task<CampaignDetailsViewModel> GetCampaign(int agencyid, int campaignid);



        ListCampaignViewModel Search(string kw, CampaignType? type, CampaignStatus? status, int index, int pagesize);
        ListCampaignViewModel Search(string kw, CampaignType? type, CampaignStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);


        ListCampaignWithAccountViewModel GetCampaignAccountByStatus(CampaignAccountStatus status, int pageindex, int pagesize);
        ListCampaignWithAccountViewModel GetCampaignAccountByStatus(CampaignAccountStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);

        ListCampaignWithAccountViewModel GetCampaignAccountByAccount(CampaignAccountStatus? status, int accountid, int pageindex, int pagesize);
        ListCampaignWithAccountViewModel GetCampaignAccountByAccount(CampaignAccountStatus? status, int accountid, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);

        Task<CampaignDetailsViewModel> GetCampaign(int campaignid);

        //Task<ListCampaignWithAccountViewModel> GetListCampaignByAllAccount(int type, string keyword, int page, int pagesize);





    }
}
