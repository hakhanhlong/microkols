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

            TotalOriginalChargeAmount = campaign.ToOriginalServiceChargeAmount(campaignAccounts, campaignOptions);
            AmountSeparateServiceCharge = TotalOriginalChargeAmount.ToServiceCharge(campaign.ServiceChargePercent);
            AmountSeparateVAT = AmountSeparateServiceCharge.ToServiceChargeWithVAT(campaign.ServiceVATPercent??0);

            ServiceChargePercent = campaign.ServiceChargePercent;
            ServiceVATPercent = campaign.ServiceVATPercent??0;

        }

        public int ServiceChargePercent { get; set; }
        public int ServiceVATPercent { get; set; }

        public long TotalOriginalChargeAmount { get; set; }
        public long AmountSeparateServiceCharge { get; set; }

        public long AmountSeparateVAT { get; set; }

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

        public bool IsValidToProcess
        {
            get
            {
                return TotalChargeValue <= 0;
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
