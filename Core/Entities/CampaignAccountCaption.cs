using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccountCaption : BaseEntity
    { 
        public int CampaignAccountId { get; set; }
        public CampaignAccount CampaignAccount { get; set; }

        public string Content { get; set; }
        public string Note { get; set; }
        public CampaignAccountCaptionStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    public enum CampaignAccountCaptionStatus
    {
        ChoDuyet = 0,
        ChuaDuyet = 1,
        DaDuyet = 2,
    }
}
