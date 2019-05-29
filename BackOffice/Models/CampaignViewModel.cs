using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{


    public class ListCampaignViewModel
    {
        public List<CampaignViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class CampaignViewModel
    {
        public CampaignViewModel() { }


        public CampaignViewModel(Campaign c) { }
    }
}
