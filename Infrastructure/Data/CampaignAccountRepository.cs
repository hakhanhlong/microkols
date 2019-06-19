using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CampaignAccountRepository : EfRepository<CampaignAccount>, ICampaignAccountRepository
    {
     
        public CampaignAccountRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
        #region Campaign Account

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

            if(campaign.Type== CampaignType.ChangeAvatar)
            {
                accountChargeAmount = campaign.AccountChargeTime * accountChargeAmount;
            }
            else if(campaign.Type== CampaignType.ShareContentWithCaption || campaign.Type== CampaignType.ShareContent)
            {
                if (campaign.EnabledAccountChargeExtra)
                {
                    var extraCharge = accountChargeAmount * campaign.AccountChargeExtraPercent / 100;
                    accountChargeAmount = accountChargeAmount + extraCharge;
                }
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
            else 
            {
                if(campaignAccount.Status== CampaignAccountStatus.Canceled)
                {
                    campaignAccount.Status = CampaignAccountStatus.AgencyRequest;
                    campaignAccount.DateModified = DateTime.Now;
                    campaignAccount.UserModified = username;
                    campaignAccount.AccountChargeAmount = accountChargeAmount;
                    await _dbContext.SaveChangesAsync();
                    return 1;
                }

            }
            return 0;
        }
        #endregion

    }
}
