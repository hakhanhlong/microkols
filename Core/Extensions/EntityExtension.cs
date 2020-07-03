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
                CampaignAccountStatus.Unfinished,
                CampaignAccountStatus.AccountRequest,
                CampaignAccountStatus.AgencyRequest,
                CampaignAccountStatus.WaitToPay,
                CampaignAccountStatus.All
            };
            accounts = accounts.Where(m => !arrIgnoreStatus.Contains(m.Status) && m.MerchantPaidToSystem != true);

            foreach (var item in accounts)
            {
                result += campaign.GetAgencyChagreAmount(item);
            }
            //long totalAccountPrice = accounts.Select(m => m.AccountChargeAmount).Sum();
            //return totalAccountPrice;

            return result;
        }

        public static long ToAmountPayback(this Campaign campaign, IEnumerable<CampaignAccount> accounts, IEnumerable<CampaignOption> options)
        {
            long result = 0;
            
            accounts = accounts.Where(m => m.Status == CampaignAccountStatus.Unfinished);

            foreach (var item in accounts)
            {
                result += campaign.GetAgencyChagreAmount(item);
            }
            //long totalAccountPrice = accounts.Select(m => m.AccountChargeAmount).Sum();
            //return totalAccountPrice;

            return result;
        }



        public static long ToOriginalServiceChargeAmount(this Campaign campaign, IEnumerable<CampaignAccount> accounts, IEnumerable<CampaignOption> options)
        {
            long result = 0;
            var arrIgnoreStatus = new List<CampaignAccountStatus>()
            {
                CampaignAccountStatus.Canceled,
                CampaignAccountStatus.Unfinished,
                CampaignAccountStatus.AccountRequest,
                CampaignAccountStatus.AgencyRequest,
                CampaignAccountStatus.WaitToPay,
                CampaignAccountStatus.All
            };
            accounts = accounts.Where(m => !arrIgnoreStatus.Contains(m.Status) && m.MerchantPaidToSystem != true);

            foreach (var item in accounts)
            {
                result += item.AccountChargeAmount;
            }
            
            return result;
        }



        //public static int GetInfuencerAmount(this Campaign campaign, int amount)
        //{
        //    var t1 = campaign.ServiceChargePercent;
        //    return 
        //}

        public static long ToServiceCharge(this long amount, int percentage_service_charge)
        {
            var _ServiceChargePercent = percentage_service_charge; //phí dịch vụ chiến dịch
            var _AccountChargeAmount = amount;

            //var _amountServiceCharge = (_AccountChargeAmount * (100 + _ServiceChargePercent)) / 100;

            var _amountServiceCharge = (_AccountChargeAmount * _ServiceChargePercent) / 100;

            return Convert.ToInt32(_amountServiceCharge);

        }

        public static long ToServiceChargeWithVAT(this long amount, int percentage_vat)
        {
            var _VATPercent = percentage_vat; //phần trăm VAT
            var _amountVAT = (amount * _VATPercent) / 100;
            return _amountVAT;
        }


        public static int GetAgencyChagreAmount(this Campaign campaign, CampaignAccount campaignAccount)
        {

            var _ServiceChargePercent = campaign.ServiceChargePercent; //phí dịch vụ chiến dịch

            //tiền gốc
            var _AccountChargeAmount = campaignAccount.AccountChargeAmount;

            //tiền có có dịch vụ
            var _amountServiceCharge = (_AccountChargeAmount * (100 + _ServiceChargePercent)) / 100;

            var _VATPercent = campaign.ServiceVATPercent ?? 0; //phần trăm VAT

            //tiền dịch vụ có VAT
            var _amountVAT = (_amountServiceCharge * (100 + _VATPercent)) / 100;

            return Convert.ToInt32(_amountVAT); //total amount include (percentage servicecharge and vatcharge)

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

            //hxq 1988 --> Phí này với Account sẽ = phí thu nhập mong muốn
            //return campaign.AccountChargeAmount;

            //Longhk add, chỗ này phải là campaignAccount.AccountChargeAmount -> không phải campaign.AccountChargeAmount em bị nhầm chỗ này
            return campaignAccount.AccountChargeAmount; 

            /*

            var t1 = campaign.ServiceChargePercent;

            var amount = campaignAccount.AccountChargeAmount;

            var val1 = (amount * (100 - t1)) / 100;

            return Convert.ToInt32(val1);
            */
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
            long longval =  (long) amount * (100 - t1);
            double val1 = longval / 100;
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
               
                if(transaction.Type== TransactionType.CampaignServiceCashBack && transaction.Amount > 0)
                {
                    totalPaid -= transaction.Amount;
                }
                else
                {
                    totalPaid += transaction.Amount;
                }
                
            }
            return totalPaid;
        }
    }
}
