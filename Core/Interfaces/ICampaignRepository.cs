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
    public interface ICampaignRepository : IRepository<Campaign>, IAsyncRepository<Campaign>
    {

        Task<List<int>> GetCampaignIds(CampaignStatus status);
        Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id);

        int CountAll();
    }
}
