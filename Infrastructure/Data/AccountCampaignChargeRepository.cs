using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class AccountCampaignChargeRepository : EfRepository<AccountCampaignCharge>, IAccountCampaignChargeRepository
    {
        public AccountCampaignChargeRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
