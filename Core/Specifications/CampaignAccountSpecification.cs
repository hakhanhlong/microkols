﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignAccountByAgencySpecification : BaseSpecification<CampaignAccount>
    {
        public CampaignAccountByAgencySpecification(int campaignid) : base(m => m.CampaignId == campaignid)
        {
        }
    }

    public class CampaignAccountByAccountSpecification : BaseSpecification<CampaignAccount>
    {
        public CampaignAccountByAccountSpecification(int accountid, int campaignid) : base(m => m.CampaignId == campaignid && m.AccountId == accountid)
        {
            AddInclude(m => m.Account);
        }
    }
}
