using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extensions;
using Core.Models;
using System.ComponentModel.DataAnnotations;
using Common.Extensions;

namespace WebServices.ViewModels
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


            RefUrl =  campaignAccount.RefUrl;
            RefId = campaignAccount.RefId;
            RefContent = campaignAccount.RefContent;
            RefData = campaignAccount.RefDataObj;
            RefImage = campaignAccount.RefImage.ToListString();

            DateFinished = campaignAccount.DateFinished;
            Rating = campaignAccount.Rating;
            ReportStatus = campaignAccount.ReportStatus;
            ReportNote = campaignAccount.ReportNote;
            ReportImages = campaignAccount.ReportImages.ToListString();
            KPICommitted = campaignAccount.KPICommitted;

            ReviewAddress = campaignAccount.ReviewAddress;

            MerchantPaidToSystem = campaignAccount.MerchantPaidToSystem;

            IsApprovedContent = campaignAccount.IsApprovedContent;
        }
        public static List<CampaignAccountViewModel> GetList(IEnumerable<CampaignAccount> campaignAccounts)
        {
            return campaignAccounts.Select(m => new CampaignAccountViewModel(m)).ToList();
        }

        public string ReviewAddress { get; set; }
        public int KPICommitted { get; set; }
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

        public int AgencyChagreAmount { get; set; }

        public bool MerchantPaidToSystem { get; set; }

        public DateTime? DateFinished { get; set; }

        public string RefUrl { get; set; }
        public string RefId { get; set; }

        public string RefContent { get; set; }
        public List<string> RefImage { get; set; } = new List<string>();
        public object RefData { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public bool? IsApprovedContent { get; set; }
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

        public string Image { get; set; }
        public int CampaignId { get; set; }
    }


    public class SubmmitCampaignAccountChangeAvatarViewModel
    {
        public int CampaignId { get; set; }
    }

    public class UpdateCampaignAccountRefViewModel
    {
        public UpdateCampaignAccountRefViewModel()
        {

        }
        [Required( ErrorMessage ="Hãy nhập {0}")]
        [Display(Name = "Copy Link bài post trên Facebook của bạn vào đây để hoàn thành công việc")]
        //[RegularExpression("^https?://(w{3}.)?facebook.com/?$", ErrorMessage ="Không đúng định dạng Url Faceboook")]
        public string RefUrl { get; set; }


        [Display(Name = "Hình ảnh thực hiện chiến dịch")]
        public List<string> RefImage { get; set; } = new List<string>();

        public string RefId { get; set; }


        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        public int CampaignId { get; set; }
        public CampaignType CampaignType { get; set; }

    }


    public class UpdateCampaignAccountRefImagesViewModel
    {
        public UpdateCampaignAccountRefImagesViewModel()
        {

        }
     
        [Display(Name = "Hình ảnh thực hiện chiến dịch")]
        public List<string> RefImage { get; set; } = new List<string>();

        public string ImagePath { get; set; }

        public int CampaignId { get; set; }
        public CampaignType CampaignType { get; set; }

    }



    public class ReportCampaignAccountViewModel
    {

        public int Id { get; set; }
        public int CampaignId { get; set; }
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Lý do báo cáo")]
        public string Note { get; set; }

        [Display(Name = "Hình ảnh chứng minh")]
        public string Image { get; set; }

    }

    public class UpdateCampaignAccountRatingViewModel
    {

        public int Id { get; set; }
        public int CampaignId { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Đánh giá")]
        public CampaignAccountRating? Rating { get; set; }
        


    }
}
