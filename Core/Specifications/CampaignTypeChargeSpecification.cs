using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignTypeChargeSpecification : BaseSpecification<CampaignTypeCharge>
    {
        public CampaignTypeChargeSpecification(CampaignType type)
         : base(i => i.Type == type)
        {
        }

    }


}
