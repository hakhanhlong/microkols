using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignType : BaseEntityWithMeta
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Data { get; set; }


        private List<Campaign> _Campaign = new List<Campaign>();
        public IEnumerable<Campaign> Campaign => _Campaign.AsReadOnly();
    }
}
