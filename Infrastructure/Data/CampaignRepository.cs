using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
namespace Infrastructure.Data
{
    public class CampaignRepository : EfRepository<Campaign>, ICampaignRepository
    {

        public CampaignRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    
        public async Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id)
        {

            var campaign = await _dbContext.Campaign.Include(m => m.CampaignAccount).Include(m => m.CampaignOption)
                .FirstOrDefaultAsync(m => m.AgencyId == agencyid && m.Published && m.Id == id);
            if (campaign != null)
            {
                var transactions = await _dbContext.Transaction.Where(m => m.RefId == campaign.Id).ToListAsync();
                return new CampaignPaymentModel(campaign, campaign.CampaignOption,
                    campaign.CampaignAccount, transactions);
            }
            return null;

        }

        public int CountAll()
        {
            return _dbContext.Campaign.Count();
        }

    }
}
