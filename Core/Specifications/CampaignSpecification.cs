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


        public CampaignByAgencySpecification(int agencyid)
          : base(m => m.AgencyId == agencyid)
        {
            
        }


    }

    public class CampaignSearchSpecification: BaseSpecification<Campaign>
    {
        public CampaignSearchSpecification(string kw, CampaignType? type, CampaignStatus? status): base (m=> (string.IsNullOrEmpty(kw) || m.Title.Contains(kw)) 
        && (!type.HasValue || m.Type == type) && (!status.HasValue || m.Status == status))
        {}


        public CampaignSearchSpecification(string kw, CampaignType? type, CampaignStatus? status, DateTime? StartDate, DateTime? EndDate) : base(m => (string.IsNullOrEmpty(kw) || m.Title.Contains(kw))
        && (!type.HasValue || m.Type == type) && (!status.HasValue || m.Status == status) &&
        (!StartDate.HasValue || m.DateCreated.Date >= StartDate.Value.Date) && (!EndDate.HasValue || m.DateCreated.Date <= EndDate.Value.Date))
        { }


    }

    public class CampaignSpecification : BaseSpecification<Campaign>
    {
        public CampaignSpecification(int campaignid) : base(m => m.Id == campaignid)
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
