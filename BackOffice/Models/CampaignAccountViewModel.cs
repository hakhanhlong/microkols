using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class CampaignAccountViewModel
    {
        public CampaignAccountViewModel(CampaignAccount campaignAccount)
        {
            Id = campaignAccount.Id;
            Account = new AccountViewModel(campaignAccount.Account, null);
            Status = campaignAccount.Status;
            DateCreated = campaignAccount.DateCreated;
            Type = campaignAccount.Type;
            AccountChargeAmount = campaignAccount.AccountChargeAmount;
            DateModified = campaignAccount.DateModified;
            AccountId = campaignAccount.AccountId;
            CampaignId = campaignAccount.CampaignId;


            RefUrl = campaignAccount.RefUrl;
            RefId = campaignAccount.RefId;
            RefContent = campaignAccount.RefContent;
            RefData = campaignAccount.RefDataObj;

            Rating = campaignAccount.Rating;
            ReportStatus = campaignAccount.ReportStatus;
            ReportNote = campaignAccount.ReportNote;
            ReportImages = campaignAccount.ReportImages.ToListString();
        }
        public static List<CampaignAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts)
        {
            return campaignAccounts.Select(m => new CampaignAccountViewModel(m)).ToList();
        }

        public int Id { get; set; }
        public CampaignAccountRating? Rating { get; set; }
        public CampaignAccountReportStatus? ReportStatus { get; set; }
        public string ReportNote { get; set; }
        public List<string> ReportImages { get; set; }

        public AccountViewModel Account { get; set; }
        public int AccountId { get; set; }
        public int CampaignId { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; } // chi phi cho tung nguoi tham gia 


        public string RefUrl { get; set; }
        public string RefId { get; set; }
        public string RefContent { get; set; }
        public object RefData { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }


    public class CampaignWithAccountViewModel : CampaignViewModel
    {
        public CampaignWithAccountViewModel(Campaign campaign, CampaignAccount campaignAccount) : base(campaign)
        {
            CampaignAccount = new CampaignAccountViewModel(campaignAccount);
        }

        public CampaignAccountViewModel CampaignAccount { get; set; }

    }

    public class ListCampaignWithAccountViewModel
    {
        public List<CampaignWithAccountViewModel> Campaigns { get; set; }
        public PagerViewModel Pager { get; set; }
    }


}
