using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;
namespace Core.Extensions
{
    public static class EntityExtension
    {
        public static long ToServiceChargeAmount(this Campaign campaign, IEnumerable<CampaignAccount> accounts, IEnumerable<CampaignOption> options)
        {
            var arrIgnoreStatus = new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.Canceled,
                CampaignAccountStatus.Unfinished
            };
            accounts = accounts.Where(m => !arrIgnoreStatus.Contains(m.Status));
            long totalAccountPrice = accounts.Select(m => m.AccountChargeAmount).Sum();

            long serviceCharge = totalAccountPrice * campaign.ServiceChargePercent / 100;

            var total1 = totalAccountPrice + serviceCharge;
            long vat = total1 * (campaign.ServiceVATPercent ?? 0) / 100;


            return total1 + vat;
        }


      
        public static long ToTotalPaidAmount(this Campaign campaign, IEnumerable<Transaction> transactions)
        {
            var completedTransactions = transactions.Where(m => m.RefId == campaign.Id && m.Status == TransactionStatus.Completed);
            long totalPaid = 0;
            foreach (var transaction in completedTransactions)
            {
                totalPaid += transaction.Amount;
            }
            return totalPaid;
        }
    }
}
