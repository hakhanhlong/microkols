using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AccountCampaignCharge : BaseEntity
    {
        public int AccountId { get; set; }
        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; }
    }
}
