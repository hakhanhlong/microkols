using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Models;

namespace Website.ViewModels
{
    public class CampaignAccountViewModel
    {
        public CampaignAccountViewModel(CampaignAccount campaignAccount)
        {
            Account = new AccountViewModel(campaignAccount.Account);
            Status = campaignAccount.Status;
            DateCreated = campaignAccount.DateCreated;
            Type = campaignAccount.Type;
            AccountChargeAmount = campaignAccount.AccountChargeAmount;
            DateModified = campaignAccount.DateModified;
            AccountId = campaignAccount.AccountId;
        }
        public static List<CampaignAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts)
        {
            return campaignAccounts.Select(m => new CampaignAccountViewModel(m)).ToList();
        }


        public AccountViewModel Account { get; set; }
        public int AccountId { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; } // chi phi cho tung nguoi tham gia 
     
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
    
    public class ListCampaignAccountViewModel
    {
        public ListCampaignAccountViewModel()
        {

        }
        public ListCampaignAccountViewModel(ListCampaignAccount campaignAccount,int page,int pagesize)
        {
            CampaignAccounts = CampaignAccountViewModel.GetList(campaignAccount.CampaignAccounts);
            Pager = new PagerViewModel(page, pagesize, campaignAccount.Total);

        }

        public List<CampaignAccountViewModel> CampaignAccounts { get; set; }
        public PagerViewModel Pager { get; set; }
    }

}
