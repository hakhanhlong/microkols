using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignAccountContentSpecification : BaseSpecification<CampaignAccountContent>
    {
        public CampaignAccountContentSpecification(int id)
         : base(i => i.Id == id)
        {
        }


    }
    public class CampaignAccountContentByCampaignAccountIdSpecification : BaseSpecification<CampaignAccountContent>
    {
        public CampaignAccountContentByCampaignAccountIdSpecification(int campaignAccountId)
         : base(i => i.CampaignAccountId == campaignAccountId)
        {
            AddInclude(m => m.CampaignAccount);
        }

        public CampaignAccountContentByCampaignAccountIdSpecification(int campaignAccountId, CampaignAccountContentStatus status)
        : base(i => i.CampaignAccountId == campaignAccountId && i.Status== status)
        { 
        }
        public CampaignAccountContentByCampaignAccountIdSpecification(IEnumerable<int> campaignAccountId)
       : base(i => campaignAccountId.Contains(i.CampaignAccountId))
        {
            AddInclude(m => m.CampaignAccount);
        }
    }

    public class CampaignAccountContentByCampaignIdSpecification : BaseSpecification<CampaignAccountContent>
    {
        public CampaignAccountContentByCampaignIdSpecification(int campaignid)
         : base(i => i.CampaignAccount.CampaignId == campaignid)
        {
            AddInclude(m => m.CampaignAccount);
        }

        public CampaignAccountContentByCampaignIdSpecification(int campaignid, CampaignAccountContentStatus status)
       : base(i => i.CampaignAccount.CampaignId == campaignid && i.Status == status)
        {
            AddInclude(m => m.CampaignAccount);
        }
    }
}
