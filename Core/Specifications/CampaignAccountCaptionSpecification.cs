using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            AddInclude(m => m.CampaignAccount);
        }


        public CampaignAccountCaptionByCampaignAccountIdSpecification(int campaignAccountId, CampaignAccountCaptionStatus status)
        : base(i => i.CampaignAccountId == campaignAccountId && i.Status== status)
        {
        }


        public CampaignAccountCaptionByCampaignAccountIdSpecification(IEnumerable<int> campaignAccountId)
     : base(i => campaignAccountId.Contains(i.CampaignAccountId))
        {
            AddInclude(m => m.CampaignAccount);
        }
    }

    public class CampaignAccountCaptionByCampaignIdSpecification : BaseSpecification<CampaignAccountCaption>
    {
        public CampaignAccountCaptionByCampaignIdSpecification(int campaignId)
         : base(i => i.CampaignAccount.CampaignId == campaignId)
        {
            AddInclude(m => m.CampaignAccount);
        }

        public CampaignAccountCaptionByCampaignIdSpecification(int campaignId, CampaignAccountCaptionStatus status)
        : base(i => i.CampaignAccount.CampaignId == campaignId && i.Status== status)
        {
            AddInclude(m => m.CampaignAccount);
        }

    }
}
