﻿using Common;
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
        Task<int> CreateCampaignAccount(int agencyid, int campaignid, int accountid, int amount, string username);


        Task<CampaignAccount> GetCampaignAccount(int campaignid, int accountid);

        Task UpdateMerchantPaidToSystem(int id, bool paid);
    }
}
