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
            long result = 0;
            var arrIgnoreStatus = new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.Canceled,
                CampaignAccountStatus.Unfinished
            };
            accounts = accounts.Where(m => !arrIgnoreStatus.Contains(m.Status));

            foreach (var item in accounts)
            {
                result += campaign.GetAgencyChagreAmount(item);
            }
            //long totalAccountPrice = accounts.Select(m => m.AccountChargeAmount).Sum();
            //return totalAccountPrice;

            return result;
        }

        //public static int GetInfuencerAmount(this Campaign campaign, int amount)
        //{
        //    var t1 = campaign.ServiceChargePercent;
        //    return 
        //}

        public static int GetAgencyChagreAmount(this Campaign campaign, CampaignAccount campaignAccount)
        {

            var t1 = campaign.ServiceChargePercent;
            var amount = campaignAccount.AccountChargeAmount;
            var val1 = (amount * (100 + t1)) / 100;

            var t2 = campaign.ServiceVATPercent ?? 0;
            var val2 = (val1 * t2) / 100;
            return Convert.ToInt32(val1 + val2);

            //var t1 = campaign.ServiceChargePercent;
            //var t2 = campaign.ServiceVATPercent ?? 0;
            //var amount = campaignAccount.AccountChargeAmount;

            ////tien sau VAT 
            //var val1 = (amount * 100) / (100 + t2);

            //var val2 = (val1 * (100 - t1)) / 100;
            //return Convert.ToInt32(val2);
        }
        public static int GetAccountChagreAmount(this Campaign campaign, CampaignAccount campaignAccount)
        {
            var t1 = campaign.ServiceChargePercent;
            var amount = campaignAccount.AccountChargeAmount;
            var val1 = (amount * (100 - t1)) / 100;
            return Convert.ToInt32(val1);
            //var t1 = campaign.ServiceChargePercent;
            //var t2 = campaign.ServiceVATPercent;
            //var amount = campaignAccount.AccountChargeAmount;

            ////tien sau VAT 
            //var val1 = (amount * 100) / (100 + t2);

            //var val2 = (val1 * (100 - t1)) / 100;
            //return Convert.ToInt32(val2);
        }

        public static int GetAccountAmountMin(this Campaign campaign)
        {
            var t1 = campaign.ServiceChargePercent;
            var amount = campaign.AmountMin;
            var val1 = (amount * (100 - t1)) / 100;
            return Convert.ToInt32(val1);
            //var t1 = campaign.ServiceChargePercent;
            //var t2 = campaign.ServiceVATPercent;
            //var amount = campaignAccount.AccountChargeAmount;

            ////tien sau VAT 
            //var val1 = (amount * 100) / (100 + t2);

            //var val2 = (val1 * (100 - t1)) / 100;
            //return Convert.ToInt32(val2);
        }
        public static int GetAccountAmountMax(this Campaign campaign)
        {
            var t1 = campaign.ServiceChargePercent;
            var amount = campaign.AmountMax;
            var val1 = (amount * (100 - t1)) / 100;
            return Convert.ToInt32(val1);
            //var t1 = campaign.ServiceChargePercent;
            //var t2 = campaign.ServiceVATPercent;
            //var amount = campaignAccount.AccountChargeAmount;

            ////tien sau VAT 
            //var val1 = (amount * 100) / (100 + t2);

            //var val2 = (val1 * (100 - t1)) / 100;
            //return Convert.ToInt32(val2);
        }


        public static int GetAccountChagreAmount(this Models.SettingModel setting, int amount)
        {

            var val1 = (amount * (100 + setting.CampaignServiceChargePercent)) / 100;
            return Convert.ToInt32(val1);
            //var val2 = (val1 * (100 + setting.CampaignVATChargePercent)) / 100; 

            //return Convert.ToInt32(val2);
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
