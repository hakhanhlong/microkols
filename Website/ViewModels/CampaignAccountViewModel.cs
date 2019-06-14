﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Models;
using System.ComponentModel.DataAnnotations;

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
            CampaignId = campaignAccount.CampaignId;


            RefUrl =  campaignAccount.RefUrl;
            RefId = campaignAccount.RefId;
            RefContent = campaignAccount.RefContent;
            RefData = campaignAccount.RefDataObj;
        }
        public static List<CampaignAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts)
        {
            return campaignAccounts.Select(m => new CampaignAccountViewModel(m)).ToList();
        }


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
    
    public class ListCampaignAccountViewModel
    {
        public ListCampaignAccountViewModel()
        {

        }
        public ListCampaignAccountViewModel(List<CampaignAccount> campaignAccounts, int page,int pagesize,int total)
        {
            CampaignAccounts = CampaignAccountViewModel.GetList(campaignAccounts);
            Pager = new PagerViewModel(page, pagesize, total);

        }

        public List<CampaignAccountViewModel> CampaignAccounts { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class SubmitCampaignAccountRefContentViewModel
    {
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Nội dung Caption")]

        public string RefContent { get; set; }
        public int CampaignId { get; set; }
    }

    public class UpdateCampaignAccountRefViewModel
    {
        public UpdateCampaignAccountRefViewModel()
        {

        }
        [Required( ErrorMessage ="Hãy nhập {0}")]
        [Display(Name = "Đường link trên Facebook")]
        //[RegularExpression("^https?://(w{3}.)?facebook.com/?$", ErrorMessage ="Không đúng định dạng Url Faceboook")]
        public string RefUrl { get; set; }

        public string RefId { get; set; }


        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public int CampaignId { get; set; }
        public CampaignType CampaignType { get; set; }

    }

}
