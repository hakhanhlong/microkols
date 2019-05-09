using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
   
    public class CampaignTypeViewModel
    {

        public CampaignTypeViewModel()
        {

        }

        public CampaignTypeViewModel(CampaignType campaignType)
        {

            Id = campaignType.Id;
            Description = campaignType.Description;
            Name = campaignType.Name;
            Data = campaignType.Data;
            Price = campaignType.Price;
        }
        public static List<CampaignTypeViewModel> GetList(IEnumerable<CampaignType> campaignTypes)
        {

            return campaignTypes.Select(m => new CampaignTypeViewModel(m)).ToList();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Data { get; set; }
    }
}
