
using Core.Entities;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Code;

namespace Website.ViewModels
{
    public class ValidatePaymentViewModel
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

    }
    #region Payment Campaign


    public class CreateServiceChargeViewModel
    {

        public int CampaignId { get; set; }
        public string Note { get; set; }
    }
    public class PaymentCampaignViewModel
    {
        public PaymentCampaignViewModel(Campaign campaign)
        {
            Amount = campaign.ToCharge(campaign.CampaignOption);
            CampaignId = campaign.Id;
            CampaignTitle = campaign.Title;
        }

        public string CampaignTitle { get; set; }
        public int CampaignId { get; set; }
        public double Amount { get; set; }

    }

    #endregion

}
