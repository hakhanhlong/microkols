using System;
using System.Collections.Generic;
using System.Text;

namespace WebServices.ViewModels
{
    public class RequestJoinCampaignViewModel
    {
        public int CampaignId { get; set; }
        public int KPICommitted { get; set; }
        public int AccountChargeAmount { get; set; }
        public string Caption { get; set; }
        public string ReviewAddress { get; set; }
    }
}
