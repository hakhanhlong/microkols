using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignAccountTypeByCampaignSpecification : BaseSpecification<CampaignAccountType>
    {
      
        public CampaignAccountTypeByCampaignSpecification(int campaignid) : base(m=> m.CampaignId== campaignid)
        {
        }
    }


}
