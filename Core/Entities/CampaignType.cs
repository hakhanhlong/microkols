using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignType : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Data { get; set; }
    }
}
