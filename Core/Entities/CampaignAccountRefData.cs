using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccountRefDataChangeAvatar
    {
        public string Avatar { get; set; }
        public DateTime TimeUpdate { get; set; }
    }


    public class CampaignAccountRefDataShareContent
    {
        public string FacebookUrl { get; set; }
        public string Content { get; set; }
        public string CountLike { get; set; }
        public string CountShare { get; set; }
        public string CountComment { get; set; }
        public string CountReaction { get; set; }
        public DateTime TimeUpdate { get; set; }
    }

    public class CampaignAccountRefDataPostComment : CampaignAccountRefDataShareContent
    {

    }
    public class CampaignAccountRefDataShareStreamUrl : CampaignAccountRefDataShareContent
    {

    }
    public class CampaignAccountRefDataJoinEvent
    {
        public string FacebookUrl { get; set; }
        public DateTime TimeUpdate { get; set; }

    }

}
