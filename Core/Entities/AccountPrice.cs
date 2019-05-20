using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AccountPrice : BaseEntity
    {
        public int AccountId { get; set; }
        public CampaignType Type { get; set; }
        public int Price { get; set; }
    }
}
