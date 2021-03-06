﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccount : BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; } // chi phi cho tung nguoi tham gia
        public int KPICommitted { get; set; }

        public string RefImage { get; set; }
        public string RefUrl { get; set; }
        public string RefId { get; set; }
        public string RefContent { get; set; }

        public bool MerchantPaidToSystem { get; set; }

        public string RefData { get; set; }
        [NotMapped]
        public object RefDataObj
        {
            get
            {
                if (!string.IsNullOrEmpty(RefData))
                {
                    if (Type == CampaignType.ChangeAvatar)
                    {
                        return JsonConvert.DeserializeObject<List<CampaignAccountRefDataChangeAvatar>>(RefData);
                    }

                    // .....
                }
                return null;
            }

        }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public DateTime? DateFinished { get; set; }

        public CampaignAccountRating? Rating { get; set; }
        public CampaignAccountReportStatus? ReportStatus { get; set; }
        public string ReportNote { get; set; }
        public string ReportImages { get; set; }
        public string Note { get; set; }

        public bool? IsRefundToAgency { get; set; }

        public bool? IsApprovedContent { get; set; }

        public string ReviewAddress { get; set; }
    }

    public enum CampaignAccountReportStatus
    {
        Reported = 1,
    }

    public enum CampaignAccountRating
    {
        [Display(Name = "Tương tác Kém")]
        InteractiveLow = 0,
        [Display(Name = "Tương tác Bình thường")]
        InteractiveMedium = 1,
        [Display(Name = "Tương tác Tốt")]
        InteractiveHigh = 2,
    }


    public enum CampaignAccountStatus
    {
        [Display(Name = "Tất cả")]
        All = -2,

        [Display(Name = "Chờ duyệt chiến dịch")]
        WaitToPay = -1,

        [Display(Name = "Thành viên xin tham gia chiến dịch")]
        AccountRequest = 0,

        [Display(Name = "Doanh nghiệp mời tham gia chiến dịch")]
        AgencyRequest = 1,

        [Display(Name = "Đã xác nhận tham gia chiến dịch")]
        Confirmed = 2,

        [Display(Name = "Đang gửi xét duyệt")]
        SubmittedContent = 3,

        [Display(Name = "Yêu cầu sửa nội dung")]
        DeclinedContent = 31,

        [Display(Name = "Đã được duyệt nội dung")]
        ApprovedContent = 32,

        [Display(Name = "Đã duyệt và cập nhật nội dung")]
        UpdatedContent = 33,

        [Display(Name = "Đã hoàn thành")]
        Finished = 6,

        [Display(Name = "Hủy tham gia")]
        Canceled = 7,

        [Display(Name = "Doanh nghiệp từ chối Influencer")]
        AgencyCanceled = 10,

        [Display(Name = "Chưa hoàn thành")]
        Unfinished = 8,
        
        [Display(Name = "Cần xác minh thực hiện chiến dịch")]
        NeedToCheckExcecuteCampaign = 9,
        







    }
    public static class CampaignAccountExt
    {
        public static string ToAgencyText(this CampaignAccountStatus status)
        {
            return status.ToString();
        }

        public static string ToAccountText(this CampaignAccountStatus status)
        {

            if (status == CampaignAccountStatus.AccountRequest)
            {
                return "Chờ xác nhận";
            }
            else if (status == CampaignAccountStatus.AgencyRequest)
            {
                return "Doanh nghiệp chỉ định";
            }
            else if (status == CampaignAccountStatus.Canceled)
            {
                return "Đã Hủy";
            }
            else if (2 <= (int)status && 6 > (int)status)
            {
                return "Đã tham gia";
            }
            else if (status == CampaignAccountStatus.Unfinished)
            {
                return "Chưa hoàn thành";
            }

            return "";
        }
        public static string ToColorClass(this CampaignStatus status)
        {
            if (status == CampaignStatus.Started) return "warning";
            if (status == CampaignStatus.Ended || status == CampaignStatus.Locked || status == CampaignStatus.Canceled) return "danger";
            if (status == CampaignStatus.Created || status== CampaignStatus.Confirmed) return "primary";

            return "success";
        }
        public static string ToColorClass(this CampaignAccountStatus status)
        {

             if (status == CampaignAccountStatus.Canceled)
            {
                return "danger";
            }
            else if (2 <= (int)status && 6 > (int)status)
            {
                return "success";
            }
            else if (status == CampaignAccountStatus.Unfinished)
            {
                return "secondary";
            }

            return "primary";
        }

        public static int GetCountApplied(this IEnumerable<CampaignAccount> campaignAccounts)
        {

            var arrStatus = new List<CampaignAccountStatus>() {   CampaignAccountStatus.Confirmed ,
        CampaignAccountStatus.SubmittedContent ,
        CampaignAccountStatus.DeclinedContent,
        CampaignAccountStatus.ApprovedContent ,
        CampaignAccountStatus.UpdatedContent ,
        CampaignAccountStatus.Finished,
            CampaignAccountStatus.Unfinished};
            return campaignAccounts.Where(m => arrStatus.Contains(m.Status)).Count();




        }



    }
}
