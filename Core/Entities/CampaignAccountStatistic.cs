using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccountStatistic : BaseEntity
    { 
        public int CampaignAccountId { get; set; }
        public CampaignAccount CampaignAccount { get; set; }

        public DateTime Date { get; set; }
        public int CountLike { get; set; }
        public int CountShare { get; set; }
        public int CountComment { get; set; }
    }

}
