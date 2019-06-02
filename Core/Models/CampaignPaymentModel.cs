using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
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
            long servicePaidAmount, accountPaidAmount;
            TotalPaidAmount = campaign.ToTotalPaidAmount(transactions, out servicePaidAmount, out accountPaidAmount);
            ServicePaidAmount = servicePaidAmount;
            AccountPaidAmount = accountPaidAmount;
        }

        public int CampaignId { get; set; }
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
                if (AccountChargeValue < 0)
                {
                    return true;
                }

                return false;
            }
        }
    }

}
