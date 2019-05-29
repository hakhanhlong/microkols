using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Entities
{
    public class CampaignTypePrice : BaseEntity
    {
        public CampaignType Type { get; set; }
        public int ServicePrice { get; set; }
        public int AccountPrice { get; set; }
        public int AccountExtraPricePercent { get; set; }

    }

  }
