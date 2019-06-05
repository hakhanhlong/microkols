using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class AccountCampaignChargeSpecification : BaseSpecification<AccountCampaignCharge>
    {
        public AccountCampaignChargeSpecification(int id) : base(m => m.Id == id)
        {

        }

    }

    public class AccountCampaignChargeByAccountSpecification : BaseSpecification<AccountCampaignCharge>
    {
        public AccountCampaignChargeByAccountSpecification(int accountId) : base(m => m.AccountId == accountId)
        {

        }

    }

}
