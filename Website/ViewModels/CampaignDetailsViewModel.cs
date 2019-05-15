using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class CampaignDetailsViewModel : CampaignViewModel
    {
        public CampaignDetailsViewModel(Campaign campaign,
            IEnumerable<CampaignOption> campaignOptions, 
            IEnumerable<CampaignAccount> campaignAccounts, 
            IEnumerable<Transaction> transactions) : base(campaign)
        {
            Payment = new CampaignPaymentViewModel(campaign,  campaignOptions, campaignAccounts, transactions);
            Transactions = TransactionViewModel.GetList(transactions);
        }

        public CampaignPaymentViewModel Payment { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }


    }


}
