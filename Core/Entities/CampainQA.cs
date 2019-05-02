using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampainQA : BaseEntityWithMeta
    {
        public int CampaignId { get; set; }

        public int AccountId { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
