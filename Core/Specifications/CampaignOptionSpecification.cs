using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignOptionByCampaignSpecification : BaseSpecification<CampaignOption>
    {
        public CampaignOptionByCampaignSpecification(int campaignId)
         : base(i => i.CampaignId == campaignId)
        {
        }

        
    }



}
