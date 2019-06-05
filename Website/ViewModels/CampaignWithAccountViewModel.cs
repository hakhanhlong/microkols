using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class CampaignWithAccountViewModel : CampaignViewModel
    {
        public CampaignWithAccountViewModel(Campaign campaign, CampaignAccount campaignAccount) : base(campaign)
        {
            CampaignAccount = new CampaignAccountViewModel(campaignAccount);
        }

        public CampaignAccountViewModel CampaignAccount { get; set; }

    }

    public class ListCampaignWithAccountViewModel
    {
        public List<CampaignWithAccountViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }

}
