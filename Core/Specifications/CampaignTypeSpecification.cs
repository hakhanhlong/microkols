using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignTypeSpecification : BaseSpecification<CampaignType>
    {
        public CampaignTypeSpecification(int id)
         : base(i => i.Id == id)
        {
        }

    }


    public class CampaignTypePublishedSpecification : BaseSpecification<CampaignType>
    {
        public CampaignTypePublishedSpecification(int id)
         : base(i => i.Id == id && i.Published)
        {
        }

        public CampaignTypePublishedSpecification()
            : base(i => i.Published)
        {
        }
    }

}
