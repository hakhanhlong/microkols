using Common;
using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICampaignAccountRepository : IRepository<CampaignAccount>, IAsyncRepository<CampaignAccount>
    {
        Task<int> CreateAgencyRequestCampaignAccount(int agencyid, int campaignid, int accountid, string username);
    }
}
