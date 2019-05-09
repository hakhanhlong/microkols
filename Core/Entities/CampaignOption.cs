using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignOption : BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public CampaignOptionName Name { get; set; }
        public string Value { get; set; }
    }

    public enum CampaignOptionName
    {
        Category,
        AgeRange,
        Gender,
        City
    }
}
