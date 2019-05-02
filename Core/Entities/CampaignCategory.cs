using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignCategory
    {

        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
