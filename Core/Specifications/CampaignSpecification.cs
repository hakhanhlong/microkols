using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignByAgencySpecification : BaseSpecification<Campaign>
    {
        public CampaignByAgencySpecification(int agencyid, CampaignType? type, string keyword)
            : base(m => m.AgencyId == agencyid && m.Published && (!type.HasValue || m.Type == type) &&

            (string.IsNullOrEmpty(keyword) || m.Title.Contains(keyword))
            )
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccount);
        }
        public CampaignByAgencySpecification(int agencyid, int id)
           : base(m => m.AgencyId == agencyid && m.Published && m.Id == id)
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccount);
        }
    }


}
