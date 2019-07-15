using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Models
{
    public class CampaignPaymentModel
    {


        public CampaignPaymentModel()
        {

        }
        public CampaignPaymentModel(Campaign campaign, IEnumerable<CampaignOption> campaignOptions,
            IEnumerable<CampaignAccount> campaignAccounts,
            IEnumerable<Transaction> transactions)
        {
            CampaignId = campaign.Id;
            ServiceChargeAmount = campaign.ToServiceChargeAmount(campaignOptions);
            long servicePaidAmount = 0, accountPaidAmount = 0, accountChargeAmount = 0;
            TotalPaidAmount = campaign.ToTotalPaidAmount(transactions, out servicePaidAmount, out accountPaidAmount);
            ServicePaidAmount = servicePaidAmount;
            AccountPaidAmount = accountPaidAmount;


            var campaginAccountStatusArr = new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.Confirmed,
                CampaignAccountStatus.Submitted ,
                CampaignAccountStatus.Declined,
                CampaignAccountStatus.Approved,
                CampaignAccountStatus.Finished
            };
            campaignAccounts = campaignAccounts.Where(m => campaginAccountStatusArr.Contains(m.Status));

            foreach (var campaignAccount in campaignAccounts)
            {
                var campaignAccountAmount = 0;
                if (campaignAccount.Account.Type == AccountType.Regular)
                {
                    campaignAccountAmount = campaign.AccountChargeAmount;
                }
                else
                {
                    campaignAccountAmount = campaignAccount.AccountChargeAmount;
                }

                accountChargeAmount += campaignAccountAmount;
                //if (campaign.EnabledAccountChargeExtra)
                //{
                //    accountChargeAmount += campaignAccountAmount * campaign.AccountChargeExtraPercent / 100;
                //}
            }

            AccountChargeAmount = accountChargeAmount;
        }

        public int CampaignId { get; set; }
        public int CampaignCode { get; set; }
        public long ServiceChargeAmount { get; set; } = 0;
        public long AccountChargeAmount { get; set; } = 0;
        public long TotalChargeAmount { get { return AccountChargeAmount + ServiceChargeAmount; } }

        public long ServicePaidAmount { get; set; }
        public long AccountPaidAmount { get; set; }
        public long TotalPaidAmount { get; set; }

        public long ServiceChargeValue
        {
            get
            {
                return ServiceChargeAmount - ServicePaidAmount;
            }
        }
        public long AccountChargeValue
        {
            get
            {
                return AccountChargeAmount - AccountPaidAmount;
            }
        }
        public long TotalChargeValue { get { return ServiceChargeValue + AccountChargeValue; } }

        public bool IsValidServiceCharge
        {
            get
            {
                if (ServiceChargeValue > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public bool IsValidAccountCharge
        {
            get
            {
                if (AccountChargeValue > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }

}
