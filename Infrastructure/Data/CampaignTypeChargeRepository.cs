using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class CampaignTypeChargeRepository: EfRepository<CampaignTypeCharge>, ICampaignTypeChargeRepository
    {
        public CampaignTypeChargeRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
