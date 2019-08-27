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
            TotalChargeAmount = campaign.ToServiceChargeAmount(campaignAccounts, campaignOptions);
            CampaignCode = campaign.Code;
            TotalPaidAmount = campaign.ToTotalPaidAmount(transactions);
        }

        public int CampaignId { get; set; }
        public string CampaignCode { get; set; }
        public long TotalChargeAmount { get; set; } = 0;


        public long TotalPaidAmount { get; set; }

        public bool IsValid
        {
            get
            {
                return TotalChargeValue != 0;
            }
        }

        public long TotalChargeValue
        {
            get
            {
                return TotalChargeAmount - TotalPaidAmount;

            }
        }



    }

}
