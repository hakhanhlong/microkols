using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebServices.ViewModels
{
    public class CampaignDetailRevenuePieChartViewModel
    {
        public CampaignDetailRevenuePieChartViewModel(TransactionCampaignRevenue entity)
        {
            TotalCampaignServiceCharge = entity.TotalCampaignServiceCharge;
            TotalCampaignServiceCashback = entity.TotalCampaignServiceCashback;
            TotalCampaignAccountPayback = entity.TotalCampaignAccountPayback;
            TotalCampaignRevenue = entity.TotalCampaignRevenue;
        }

        public long TotalCampaignServiceCharge { get; set; }
        public long TotalCampaignServiceCashback { get; set; }
        public long TotalCampaignAccountPayback { get; set; }
        public long TotalCampaignRevenue { get; set; }
    }
}
