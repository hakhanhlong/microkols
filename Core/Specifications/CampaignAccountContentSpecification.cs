using Core.Entities;
using System;
using System.Collections.Generic;
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

    }

    public class CampaignAccountContentByCampaignIdSpecification : BaseSpecification<CampaignAccountContent>
    {
        public CampaignAccountContentByCampaignIdSpecification(int campaignid)
         : base(i => i.CampaignAccount.CampaignId == campaignid)
        {
            AddInclude(m => m.CampaignAccount);
        }

    }
}
