using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;

namespace Website.ViewModels
{
    public class CampaignAccountViewModel
    {
        public CampaignAccountViewModel(CampaignAccount campaignAccount)
        {
            Account = new AccountViewModel(campaignAccount.Account);
            Status = campaignAccount.Status;
            Data = campaignAccount.Data;
            DateCreated = campaignAccount.DateCreated;
        }
        public static List<CampaignAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts)
        {
            return campaignAccounts.Select(m => new CampaignAccountViewModel(m)).ToList();
        }


        public AccountViewModel Account { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public string Data { get; set; }
        public DateTime DateCreated { get; set; }
    }


}
