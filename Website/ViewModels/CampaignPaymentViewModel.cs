using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;

namespace Website.ViewModels
{
    public class CampaignPaymentViewModel
    {
        private long _servicePaidAmount;
        private long _accountPaidAmount;

        public CampaignPaymentViewModel()
        {

        }
        public CampaignPaymentViewModel(Campaign campaign,IEnumerable<CampaignOption> campaignOptions,
            IEnumerable<CampaignAccount> campaignAccounts,
            IEnumerable<Transaction> transactions)
        {
            CampaignId = campaign.Id;
            ServiceChargeAmount = campaign.ToServiceChargeAmount(campaignOptions);
            TotalPaidAmount = campaign.ToTotalPaidAmount(transactions,out _servicePaidAmount, out _accountPaidAmount);
        }


        public int CampaignId { get; set; }
        public long ServiceChargeAmount { get; set; } = 0;
        public long AccountChargeAmount { get; set; } = 0;
        public long TotalChargeAmount { get { return AccountChargeAmount + ServiceChargeAmount; } }

        public long ServicePaidAmount { get => _servicePaidAmount; set => _servicePaidAmount = value; }
        public long AccountPaidAmount { get => _accountPaidAmount; set => _accountPaidAmount = value; }
        public long TotalPaidAmount { get; set; }

        public bool IsValidServiceCharge
        {
            get
            {
                if(ServiceChargeAmount< ServicePaidAmount)
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
                if (ServiceChargeAmount < ServicePaidAmount)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
