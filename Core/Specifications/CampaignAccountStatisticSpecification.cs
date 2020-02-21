using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class CampaignAccountStatisticSpecification : BaseSpecification<CampaignAccountStatistic>
    {
        public CampaignAccountStatisticSpecification(int id)
         : base(i => i.Id == id)
        {
        }


    }
    public class CampaignAccountStatisticByCampaignAccountIdSpecification : BaseSpecification<CampaignAccountStatistic>
    {
        public CampaignAccountStatisticByCampaignAccountIdSpecification(int campaignAccountId)
         : base(i => i.CampaignAccountId == campaignAccountId)
        {
        }

    }

    public class CampaignAccountStatisticByCampaignIdSpecification : BaseSpecification<CampaignAccountStatistic>
    {
        public CampaignAccountStatisticByCampaignIdSpecification(int campaignId)
         : base(i => i.CampaignAccount.CampaignId == campaignId)
        {
            AddInclude(m => m.CampaignAccount);
        }

    }
}
