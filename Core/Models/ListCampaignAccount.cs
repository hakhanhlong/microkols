using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class ListCampaignAccount
    {
        public List<CampaignAccount> CampaignAccounts { get; set; }
        public int Total { get; set; }
    }
}
