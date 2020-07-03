using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Models
{
    public class CampaignPaymentModel
    {


        public CampaignPaymentModel()
        {

        }
        public CampaignPaymentModel(Campaign campaign, IEnumerable<CampaignOption> campaignOptions,
            IEnumerable<CampaignAccount> campaignAccounts,
            IEnumerable<Transaction> transactions)
        {
            CampaignId = campaign.Id;

            
            CampaignCode = campaign.Code;

            TotalPaidAmount = campaign.ToTotalPaidAmount(transactions); // tổng tiền đã trả

            // tiền chưa tính phần trăm dịch vụ
            TotalOriginalChargeAmount = campaign.ToOriginalServiceChargeAmount(campaignAccounts, campaignOptions);

            // tiền dịch vụ
            AmountSeparateServiceCharge = TotalOriginalChargeAmount.ToServiceCharge(campaign.ServiceChargePercent);

            // tiền gốc + phần trăm dịch vụ
            AmountSeparateVAT = (TotalOriginalChargeAmount + AmountSeparateServiceCharge);//.ToServiceChargeWithVAT(campaign.ServiceVATPercent??0);


            //tổng tiền doanh nghiệp cần phải trả cho hệ thống
            TotalChargeAmount = campaign.ToServiceChargeAmount(campaignAccounts, campaignOptions);

            //tổng tiền hệ thống phải trả lại cho doanh nghiệp
            TotalPayback = campaign.ToAmountPayback(campaignAccounts, campaignOptions);




            ServiceChargePercent = campaign.ServiceChargePercent; // phần trăm tính tiền dịch vụ
            ServiceVATPercent = campaign.ServiceVATPercent??0; // tiền VAT

            CampaignAccounts = campaignAccounts.ToList();

        }

        public List<CampaignAccount> CampaignAccounts { get; set; }

        public int ServiceChargePercent { get; set; }
        public int ServiceVATPercent { get; set; }

        public long TotalOriginalChargeAmount { get; set; }
        public long AmountSeparateServiceCharge { get; set; }

        public long AmountSeparateVAT { get; set; }

        public int CampaignId { get; set; }
        public string CampaignCode { get; set; }
        public long TotalChargeAmount { get; set; } = 0;


        //khi có influencer nào ko thực hiện chiến dịch thì sẽ trả lại tiền cho doanh nghiệp.
        public long TotalPayback { get; set; } = 0;


        public long TotalPaidAmount { get; set; }

        public bool IsValid
        {
            get
            {
                return TotalChargeValue != 0;
            }
        }

        public bool IsValidToProcess
        {
            get
            {
                return TotalChargeValue <= 0;
            }
        }

        public long TotalChargeValue
        {
            get
            {
                return TotalChargeAmount - TotalPaidAmount;

            }
        }



    }

}
