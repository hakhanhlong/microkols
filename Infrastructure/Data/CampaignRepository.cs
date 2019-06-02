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
        #region Campaign Account
        public async Task<ListCampaignAccount> GetCampaignAccounts(int campaignid, int page, int pagesize)
        {
            var query = _dbContext.CampaignAccount.Where(m => m.CampaignId == campaignid);
            var total = await query.CountAsync();
            var list = await query.OrderByDescending(m => m.DateModified).Include(m => m.Account).GetPagedAsync(page, pagesize);
            return new ListCampaignAccount()
            {
                CampaignAccounts = list,
                Total = total
            };
        }
        public async Task<int> CreateAgencyRequestCampaignAccount(int agencyid, int campaignid, int accountid, string username)
        {
            var campaign = await _dbContext.Campaign.FirstOrDefaultAsync(m => m.Id == campaignid && m.AgencyId == agencyid);
            if (campaign == null)
                return -1;
            var account = await _dbContext.Account.FirstOrDefaultAsync(m => m.Id == accountid);
            if (account == null) return -1;

            var accountChargeAmount = campaign.AccountChargeAmount;
            //check la KOLs
            if (account.Type != AccountType.Regular)
            {
                var accountPrice = await _dbContext.AccountCampaignCharge.Where(m => m.AccountId == account.Id && m.Type == campaign.Type).FirstOrDefaultAsync();
                if (accountPrice == null) return -1;
                accountChargeAmount = accountPrice.AccountChargeAmount;
            }
            var campaignAccount = await _dbContext.CampaignAccount.FirstOrDefaultAsync(m => m.CampaignId == campaign.Id && m.AccountId == account.Id);
            if (campaignAccount == null)
            {
                campaignAccount = new CampaignAccount()
                {
                    RefData = string.Empty,
                    RefId = string.Empty,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    CampaignId = campaign.Id,
                    AccountId = account.Id,
                    RefUrl = string.Empty,
                    AccountChargeAmount = accountChargeAmount,
                    Status = CampaignAccountStatus.AgencyRequest,
                    Type = campaign.Type,
                    UserCreated = username,
                    UserModified = username,
                };
                await _dbContext.CampaignAccount.AddAsync(campaignAccount);
                await _dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }
        #endregion
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
