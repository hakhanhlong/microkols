using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;
namespace Core.Extensions
{
    public static class EntityExtension
    {
        public static long ToCharge(this Campaign campaign, IEnumerable<CampaignOption> options)
        {
            return GetCampaignCharge(campaign.CampaignTypeCharge, campaign.ServiceChargePercent, campaign.ExtraChargePercent, options.Count());
        }


        public static long GetCampaignCharge(int campaignTypeCharge, int serviceChargePercent, int extraChargePercent, int countOption )
        {
            var result = campaignTypeCharge + countOption* extraChargePercent * campaignTypeCharge/100;
            return result* serviceChargePercent/100 + result;
        }
    }
}
