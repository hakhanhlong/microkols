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
        public CampaignDetailsViewModel(Campaign campaign, IEnumerable<Transaction> transactions) : base(campaign)
        {
            Price = campaign.ToCharge(campaign.CampaignOption);
            PaidPrice = campaign.ToPaidPrice(transactions);
        }

        public long Price { get; set; }
        public long PaidPrice { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }


    }


}
