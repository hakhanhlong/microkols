using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignTypePriceSpecification : BaseSpecification<CampaignTypePrice>
    {
        public CampaignTypePriceSpecification(CampaignType type)
         : base(i => i.Type == type)
        {
        }

    }


}
