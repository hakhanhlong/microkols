using Core.Entities;
using Core.Extensions;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
    public class CampaignDetailsViewModel : CampaignViewModel
    {
        public CampaignDetailsViewModel(Campaign campaign,
            IEnumerable<CampaignOption> campaignOptions, 
            IEnumerable<CampaignAccount> campaignAccounts, 
            IEnumerable<Transaction> transactions) : base(campaign)
        {
            EnabledAccountChargeExtra = campaign.EnabledAccountChargeExtra;

            AccountChargeTime = campaign.AccountChargeTime;
            SystemNote = campaign.SystemNote;


            //campaignAccounts = campaignAccounts.Where(m => m.Status != CampaignAccountStatus.Canceled);
            Payment = new CampaignPaymentModel(campaign,  campaignOptions, campaignAccounts, transactions);

            Transactions = TransactionViewModel.GetList(transactions);

            CampaignAccounts = CampaignAccountViewModel.GetList(campaignAccounts);
        }

        public bool EnabledAccountChargeExtra { get; set; }
        public int AccountChargeTime { get; set; }

        public string SystemNote { get; set; }


        public CampaignPaymentModel Payment { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }

        public List<CampaignAccountViewModel> CampaignAccounts { get; set; }

      

    }
    
    public class CampaignAccountCountingViewModel
    {

        public int TongNguoiThamGia { get; set; } 
        public int TongNguoi { get; set; }
        public int TongCaptionDaDuyet { get; set; }
        public int TongCaptionCanDuyet { get; set; }
        public int TongCaption { get; set; }


        public int TongContentDaDuyet { get; set; }
        public int TongContentCanDuyet { get; set; }
        public int TongContent { get; set; }
    }

    
}
