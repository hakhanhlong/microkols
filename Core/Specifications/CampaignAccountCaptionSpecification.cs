using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class CampaignAccountCaptionSpecification : BaseSpecification<CampaignAccountCaption>
    {
        public CampaignAccountCaptionSpecification(int id)
         : base(i => i.Id == id)
        {
        }


    }
    public class CampaignAccountCaptionByCampaignAccountIdSpecification : BaseSpecification<CampaignAccountCaption>
    {
        public CampaignAccountCaptionByCampaignAccountIdSpecification(int campaignAccountId)
         : base(i => i.CampaignAccountId == campaignAccountId)
        {
        }

    }

    public class CampaignAccountCaptionByCampaignIdSpecification : BaseSpecification<CampaignAccountCaption>
    {
        public CampaignAccountCaptionByCampaignIdSpecification(int campaignId)
         : base(i => i.CampaignAccount.CampaignId == campaignId)
        {
            AddInclude(m => m.CampaignAccount);
        }

    }
}
