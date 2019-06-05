using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccountType : BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public AccountType AccountType { get; set; }


    }
}
