using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
   
    public class CampaignTypePriceViewModel
    {

        public CampaignTypePriceViewModel()
        {

        }

        public CampaignTypePriceViewModel(CampaignTypePrice campaignType)
        {

            Id = campaignType.Id;
            Type = campaignType.Type;
            ServicePrice = campaignType.ServicePrice;
            AccountPrice = campaignType.AccountPrice;
            AccountExtraPricePercent = campaignType.AccountExtraPricePercent;
        }
        public static List<CampaignTypePriceViewModel> GetList(IEnumerable<CampaignTypePrice> campaignTypes)
        {

            return campaignTypes.Select(m => new CampaignTypePriceViewModel(m)).ToList();

        }

        public int Id { get; set; }
        public CampaignType Type { get; set; }
        public int ServicePrice { get; set; }
        public int AccountPrice { get; set; }
        public int AccountExtraPricePercent { get; set; }
    }
}
