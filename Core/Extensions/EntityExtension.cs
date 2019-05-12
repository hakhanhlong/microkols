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

        public static long ToPaidPrice(this Campaign campaign, IEnumerable<Transaction> transactions)
        {
            return transactions.Where(m => m.RefId == campaign.Id && m.Status== TransactionStatus.Completed).Select(m => m.Amount).DefaultIfEmpty(0).Sum();
        }
    }
}
