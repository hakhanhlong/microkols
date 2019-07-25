using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignByAgencySpecification : BaseSpecification<Campaign>
    {
        public CampaignByAgencySpecification(int agencyid, CampaignType? type, CampaignStatus? status, string keyword)
            : base(m => m.AgencyId == agencyid && m.Published && (!type.HasValue || m.Type == type) && (!status.HasValue || m.Status == status) &&

            (string.IsNullOrEmpty(keyword) || m.Title.Contains(keyword))
            )
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccount);
            AddInclude(m => m.CampaignAccountType);
        }

        public CampaignByAgencySpecification(int agencyid, int id)
           : base(m => m.AgencyId == agencyid && m.Published && m.Id == id)
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccount);
            AddInclude($"{nameof(Campaign.CampaignAccount)}.{nameof(CampaignAccount.Account)}");// m => m.CampaignAccount);
            AddInclude(m => m.CampaignAccountType);
        }
    }

    public class CampaignByAccountSpecification : BaseSpecification<Campaign>
    {
        public CampaignByAccountSpecification(int accountid, string kw)
          : base(m => m.CampaignAccount.Any(n => n.AccountId == accountid) && (string.IsNullOrEmpty(kw) || m.Description.Contains(kw)))
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccountType);
        }

        public CampaignByAccountSpecification(int accountid, int id)
           : base(m => m.CampaignAccount.Any(n => n.AccountId == accountid) && m.Published && m.Id == id)
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccountType);
        }
    }

    public class CampaignExpiredFeedbackTimeSpecification : BaseSpecification<Campaign>
    {
        public CampaignExpiredFeedbackTimeSpecification()
          : base(m => m.Status == CampaignStatus.Created && m.AccountFeedbackBefore.HasValue && m.AccountFeedbackBefore.Value < DateTime.Now)
        {
        }


    }


}
