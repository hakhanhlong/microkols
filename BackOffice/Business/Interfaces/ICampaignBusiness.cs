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
        ListCampaignViewModel GetListCampaign(int index, int pagesize);

        Task<CampaignDetailsViewModel> GetCampaign(int agencyid, int campaignid);

        ListCampaignViewModel Search(string kw, CampaignType? type, CampaignStatus? status, int index, int pagesize);


    }
}
