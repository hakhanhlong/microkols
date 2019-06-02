using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Entities
{
    public class CampaignTypeCharge : BaseEntity
    {
        public CampaignType Type { get; set; }
        public int ServiceChargeAmount { get; set; }
        public int AccountChargeAmount { get; set; }
        public int AccountChargeExtraPercent { get; set; }

    }

  }
