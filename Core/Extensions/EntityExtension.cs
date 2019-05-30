using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;
namespace Core.Extensions
{
    public static class EntityExtension
    {
        public static long ToServiceChargeAmount(this Campaign campaign, IEnumerable<CampaignOption> options)
        {
            return GetCampaignServiceCharge(campaign.ServicePrice, campaign.ServiceChargePercent,
                campaign.ExtraOptionChargePercent, options.Count());
        }

        public static long GetCampaignServiceCharge(int servicePrice, int serviceChargePercent,
            int extraOptionChargePercent, int countOption)
        {
            var result = servicePrice + countOption * extraOptionChargePercent * servicePrice / 100;
            return result * serviceChargePercent / 100 + result;
        }


        public static long ToTotalPaidAmount(this Campaign campaign, IEnumerable<Transaction> transactions)
        {
            long accountPaid = 0;
            long servicePaid = 0;
            return campaign.ToTotalPaidAmount(transactions, out servicePaid, out accountPaid);
        }
        public static long ToTotalPaidAmount(this Campaign campaign, IEnumerable<Transaction> transactions, out long servicePaidAmount, out long accountPaidAmount)
        {
            var completedTransactions = transactions.Where(m => m.RefId == campaign.Id && m.Status == TransactionStatus.Completed);

            long totalPaid = 0;
            long accountPaid = 0;
            long servicePaid = 0;
            foreach (var transaction in completedTransactions)
            {
                totalPaid += transaction.Amount;
                if (transaction.Type == TransactionType.CampaignServiceCharge)
                {
                    accountPaid += transaction.Amount;
                }
                else if (transaction.Type == TransactionType.CampaignAccountCharge)
                {
                    servicePaid += transaction.Amount;
                }
            }
            servicePaidAmount = servicePaid;
            accountPaidAmount = accountPaid;
            return totalPaid;
        }
    }
}
