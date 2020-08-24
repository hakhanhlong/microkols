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

        public async Task UpdateMerchantPaidToSystem(int id, bool paid)
        {
            var campaignAccount = await _dbContext.CampaignAccount.FirstOrDefaultAsync(m => m.Id == id);
            campaignAccount.MerchantPaidToSystem = paid;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CampaignAccount> GetCampaignAccount(int campaignid, int accountid)
        {
            var campaignAccount = await _dbContext.CampaignAccount.FirstOrDefaultAsync(m => m.AccountId == accountid && m.CampaignId == campaignid);

            return campaignAccount;

        }
        #region Campaign Account

        public async Task<int> CreateCampaignAccount(int agencyid, int campaignid, int accountid, int amount, string username)
        {

            var campaign = await _dbContext.Campaign.FirstOrDefaultAsync(m => m.Id == campaignid && m.AgencyId == agencyid);
            if (campaign == null)
                return -1;
            /*
           var account = await _dbContext.Account.FirstOrDefaultAsync(m => m.Id == accountid);
           if (account == null) return -1;

           var accountChargeAmount = campaign.AccountChargeAmount;
           //check la KOLs
           if (account.Type != AccountType.Regular)
           {
               var accountPrice = await _dbContext.AccountCampaignCharge.Where(m => m.AccountId == account.Id && m.Type == campaign.Type).FirstOrDefaultAsync();
               if (accountPrice == null)
               {
                   accountChargeAmount = amount;
               }
               else
               {
                   accountChargeAmount = accountPrice.AccountChargeAmount;
                   if (amount > accountChargeAmount)
                   {
                       accountChargeAmount = amount;
                   }
               }
           }

           if (campaign.Type == CampaignType.ChangeAvatar)
           {
               accountChargeAmount = campaign.AccountChargeTime * accountChargeAmount;
           }
           else if (campaign.Type == CampaignType.ShareContentWithCaption || campaign.Type == CampaignType.ShareContent)
           {
               if (campaign.EnabledAccountChargeExtra)
               {
                   var extraCharge = accountChargeAmount * campaign.AccountChargeExtraPercent / 100;
                   accountChargeAmount = accountChargeAmount + extraCharge;
               }
           }
           */
            var accountChargeAmount = amount;
            if(accountChargeAmount== 0)
            {

                var charge = await _dbContext.CampaignTypeCharge.FirstOrDefaultAsync(m => m.Type == campaign.Type);
                if(charge!= null)
                {
                    accountChargeAmount = charge.AccountChargeAmount;
                }

            }
            //var settingcharge = await _dbContext.Setting.Where(m => m.Name == SettingName.CampaignServiceChargePercent).FirstOrDefaultAsync();
            //if (settingcharge != null)
            //{
            //    var percent = 100 - int.Parse(settingcharge.Value);

            //    accountChargeAmount = amount * percent / 100;
            //}


            var campaignAccount = await _dbContext.CampaignAccount.FirstOrDefaultAsync(m => m.CampaignId == campaign.Id && m.AccountId == accountid);
            if (campaignAccount == null)
            {
                campaignAccount = new CampaignAccount()
                {
                    RefData = string.Empty,
                    RefId = string.Empty,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    CampaignId = campaign.Id,
                    AccountId = accountid,
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
                if (campaignAccount.Status == CampaignAccountStatus.Canceled)
                {
                    campaignAccount.Status = CampaignAccountStatus.WaitToPay;
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
