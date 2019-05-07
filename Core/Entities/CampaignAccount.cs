using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccount
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public string Data { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    public enum CampaignAccountStatus
    {
        Request = 0,
        Joined = 1
    }
}
