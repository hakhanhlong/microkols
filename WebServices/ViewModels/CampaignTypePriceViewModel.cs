using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{
   
    public class CampaignTypeChargeViewModel
    {

        public CampaignTypeChargeViewModel()
        {

        }

        public CampaignTypeChargeViewModel(CampaignTypeCharge campaignType)
        {
            Id = campaignType.Id;
            Type = campaignType.Type;
            ServiceChargeAmount = campaignType.ServiceChargeAmount;
            AccountChargeAmount = campaignType.AccountChargeAmount;
            AccountChargeExtraPercent = campaignType.AccountChargeExtraPercent;
        }
        public static List<CampaignTypeChargeViewModel> GetList(IEnumerable<CampaignTypeCharge> campaignTypes)
        {
            return campaignTypes.Select(m => new CampaignTypeChargeViewModel(m)).ToList();
        }

        public int Id { get; set; }

        public CampaignType Type { get; set; }
        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }
    }
}
