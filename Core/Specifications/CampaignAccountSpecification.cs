using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class CampaignAccountSpecification : BaseSpecification<CampaignAccount>
    {
        public CampaignAccountSpecification(int campaignid) : base(m => m.CampaignId == campaignid)
        {
        }
        public CampaignAccountSpecification(int campaignid, CampaignAccountStatus status ) : base(m => m.CampaignId == campaignid && m.Status== status)
        {
        }

        public CampaignAccountSpecification(int campaignid, IEnumerable<CampaignAccountStatus> status, IEnumerable<CampaignAccountStatus> ignoreStatus)
           : base(m => m.CampaignId == campaignid && (status == null || status.Contains(m.Status)) &&
           (ignoreStatus == null  || !ignoreStatus.Contains(m.Status)))
        {

        }
    }


    public class CampaignAccountByAgencySpecification : BaseSpecification<CampaignAccount>
    {
        public CampaignAccountByAgencySpecification(int campaignid) : base(m => m.CampaignId == campaignid)
        {
        }

        public CampaignAccountByAgencySpecification(int campaignid, CampaignAccountStatus status) : base(m => m.CampaignId == campaignid && m.Status == status)
        {
        }
    }




    public class ConfirmedCampaignAccountSpecification : BaseSpecification<CampaignAccount>
    {
        public ConfirmedCampaignAccountSpecification(int campaignid) : base(m => m.CampaignId == campaignid 
        && m.Status != CampaignAccountStatus.AccountRequest && m.Status != CampaignAccountStatus.AgencyRequest)
        {
        }


    }
    public class CampaignAccountByAccountSpecification : BaseSpecification<CampaignAccount>
    {

        public CampaignAccountByAccountSpecification(int accountid, int campaignid) 
            : base(m => m.CampaignId == campaignid && m.AccountId == accountid)
        {
            AddInclude(m => m.Account);
        }

        public CampaignAccountByAccountSpecification(int accountid, IEnumerable<CampaignAccountStatus> arrStatus)
          : base(m => arrStatus.Contains(m.Status) && m.AccountId == accountid)
        {

        }
    }

    

}
