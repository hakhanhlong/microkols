using Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core.Entities
{
    public class Notification : BaseEntity
    {
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public NotificationType Type { get; set; }
        public string Data { get; set; }
        public int DataId { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        
        public DateTime DateCreated { get; set; }
        public NotificationStatus Status { get; set; }
    }
    public enum NotificationType
    {
        AgencyRequestJoinCampaign = 0,
        AgencyConfirmJoinCampaign = 1,
        AccountRequestJoinCampaign  = 2, 
        AccountConfirmJoinCampaign = 3,      
        
        AccountSubmitCampaignRefContent = 4,
        AccountFinishCampaignRefContent =5,
        AgencyApproveCampaignRefContent = 6,
        AgencyDeclineCampaignRefContent = 7,
        AgencyUpdatedCampaignRefContent = 8,
        AgencyCancelAccountJoinCampaign = 9,

        AccountDeclineJoinCampaign = 10,
        SystemUpdateUnfinishedAccountCampaign  = 11,
        SystemUpdateCanceledAccountCampaign = 111,

        // transaction deposite, withdraw for agency

        TransactionDepositeApprove = 12, 
        TransactionDepositeProcessing = 13,
        TransactionDepositeCancel = 14,

        TransactionWithdrawApprove = 121,
        TransactionWithdrawProcessing = 132,
        TransactionWithdrawCancel = 143,

        TransactionAccountRefundToAgency = 144,

        TransactionCampaignServiceCashBackApprove = 145,
        TransactionCampaignServiceCashBackProcessing = 146,
        TransactionCampaignServiceCashBackCancel = 147,

        ExcecutedPaymentToAccountBanking = 22,

        //##########################################



        CampaignStarted = 15,
        CampaignCantStarted = 16,
        CampaignEnded = 17,
        CampaignCompleted = 18,
        CampaignCanceled = 19,
        CampaignConfirmed = 20,
        CampaignError = 21,        
        //####### longhk add ################
        CampaignCreated = 23,
        CampaignLocked  = 24,        
        //###################################


        AccountSubmitCampaignCaption = 30,
        AgencyApproveCampaignCaption = 31,
        AgencyDeclineCampaignCaption = 32,
        AgencyUpdatedCampaignCaption = 33,


        AccountSubmitCampaignContent = 40,
        AgencyApproveCampaignContent = 41,
        AgencyDeclineCampaignContent = 42,
        AgencyUpdatedCampaignContent = 43,


        //####### longhk add ####################

        AgencyWalletDeposit = 46,
        AgencyWalletWithDraw = 47,
        AgencyPayCampaignService = 48,

        AccountSendVerify = 50,
        AccountVerifyDenied = 51,
        AccountVerifySuccess = 52,

        AgencyRequestWithdrawFromCampaign = 55,

        //#######################################


    }

    public enum NotificationTypeGroup
    {
       
        [Display(Name="Hệ thống")]
        System,
        [Display(Name = "Chiến dịch")]
        Campaign,
        [Display(Name = "Thanh toán")]
        Payment
    }
    public enum NotificationStatus
    {
        Created = 0,
        Checked = 1
    }

    public static class NotificationExt
    {

        public static string NotificationTypeToText(this NotificationType type)
        {
            string text = "Not Yet Define";

            if(type == NotificationType.AgencyWalletDeposit)
            {
                text = "Doanh nghiệp nạp tiền ví";
            }
            if (type == NotificationType.AgencyWalletWithDraw)
            {
                text = "Doanh nghiệp rút tiền ví";
            }
            else if(type == NotificationType.AgencyPayCampaignService)
            {
                text = "Doanh nghiệp thanh toán phí dịch vụ chiến dịch";
            }
            else if (type == NotificationType.AgencyRequestWithdrawFromCampaign)
            {
                text = "Doanh nghiệp yêu cầu rút tiền từ chiến dịch";
            }
            else if (type == NotificationType.AccountSendVerify)
            {
                text = "Influencer gửi xác thực";
            }
            else if (type == NotificationType.CampaignCreated)
            {
                text = "Chiến dịch được tạo";
            }
            else if (type == NotificationType.CampaignLocked)
            {
                text = "Chiến dịch tạm khóa";
            }
            else if (type == NotificationType.CampaignCanceled)
            {
                text = "Chiến dịch bị hủy";
            }
            else if (type == NotificationType.CampaignStarted)
            {
                text = "Chiến dịch bắt đầu";
            }
            else if (type == NotificationType.CampaignEnded)
            {
                text = "Chiến dịch kết thúc";
            }

            return text;
        }




        public static NotificationTypeGroup ToTypeGroup(this NotificationType type)
        {

            var str = type.ToString();
            if (str.Contains("Campaign"))
            {
                return NotificationTypeGroup.Campaign;

            }
            if (str.Contains("Payment"))
            {
                return NotificationTypeGroup.Payment;

            }
            return NotificationTypeGroup.System;
        }
        public static string ToText(this NotificationTypeGroup typeGroup)
        {
            return typeGroup.ToDisplayName();
        }
        public static List<NotificationType> GetNotificationTypes(this NotificationTypeGroup? group)
        {
            if (group.HasValue)
            {

            }
            return Common.Helpers.StringHelper.GetEnumArray<NotificationType>().ToList();
        }
        public static string GetMessageText(this NotificationType type, params string[] args)
        {
            
               var message = "";
            if (type == NotificationType.SystemUpdateUnfinishedAccountCampaign)
            {
                message = "Hệ thống cập nhật bạn không hoàn thành chiến dịch {0}";
            }
            else
           if (type == NotificationType.AccountDeclineJoinCampaign)
            {
                message = "Thành viên {0} đã không đồng ý tham gia chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AccountConfirmJoinCampaign)
            {
                message = "Thành viên {0} đã đồng ý tham gia chiến dịch {1}";
            }
            else if (type == NotificationType.AccountFinishCampaignRefContent)
            {
                message = "Thành viên {0} đã thực hiện công việc chiến dịch {1}";
            }

            else if(type == NotificationType.AgencyCancelAccountJoinCampaign)
            {
                message = "Doanh nghiệp {0} đã hủy quyền tham gia dự án chiến dịch {1} của bạn";
            }
            
            else if (type == NotificationType.AgencyApproveCampaignRefContent)
            {
                message = "Doanh nghiệp {0} đã duyệt nội dung Caption chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AgencyDeclineCampaignRefContent)
            {
                message = "Doanh nghiệp {0} yêu cầu sửa lại nội dung Caption chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AgencyUpdatedCampaignRefContent)
            {
                message = "Doanh nghiệp {0} từ sửa nội dung Caption chiến dịch {1} của bạn";
            }
            
            else if (type == NotificationType.AccountSubmitCampaignRefContent)
            {
                message = "Thành viên {0} đã gửi nội dung Caption chiến dịch {1}";
            }
            
            else if (type == NotificationType.AgencyConfirmJoinCampaign)
            {
                message = "Doanh nghiệp {0} đã duyệt yêu cầu tham gia chiến dịch {1}";
            }
            else if (type == NotificationType.AccountRequestJoinCampaign)
            {
                message = "Thành viên {0} đã đề xuất được tham gia chiến dịch {1}";
            }
            else if (type == NotificationType.AgencyRequestJoinCampaign)
            {
                message = "Doanh nghiệp {0} mời bạn tham gia chiến dịch {1}";
            }
            else if (type == NotificationType.CampaignStarted)
            {
                message = "Chiến dịch {0} đã bắt đầu thực hiện";
            }
            else if (type == NotificationType.CampaignCantStarted)
            {
                message = "{0} tạm khóa chiến dịch {1} vì: {2}";
            }

            
            else if (type == NotificationType.CampaignEnded)
            {
                message = "Chiến dịch {0} đã kết thúc";
            }
            else if (type == NotificationType.CampaignCanceled)
            {
                message = "Chiến dịch {0} bị hủy vì lí do {1}";
            }
            else if (type == NotificationType.SystemUpdateCanceledAccountCampaign)
            {
                message = "Chiến dịch {0} bị hủy vì lí do {1}";
            }
            
            else if (type == NotificationType.CampaignCompleted)
            {
                message = "Chiến dịch {0} đã hoàn thành. Bạn đã được nhận {1} ";
            }

            else if (type == NotificationType.AgencyDeclineCampaignCaption)
            {
                message = "Doanh nghiệp {0} đã không duyệt nội dung Caption chiến dịch {1} của bạn";
            }

            else if (type == NotificationType.AgencyApproveCampaignCaption)
            {
                message = "Doanh nghiệp {0} đã duyệt nội dung Caption chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AgencyUpdatedCampaignCaption)
            {
                message = "Doanh nghiệp {0} thêm ghi chú Caption chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AccountSubmitCampaignCaption)
            {
                message = "Thành viên {0} đã gửi nội dung Caption chiến dịch {1}";
            }

            else if (type == NotificationType.AgencyDeclineCampaignContent)
            {
                message = "Doanh nghiệp {0} đã không duyệt nội dung chiến dịch {1} của bạn";
            }

            else if (type == NotificationType.AgencyApproveCampaignContent)
            {
                message = "Doanh nghiệp {0} đã duyệt nội dung  chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AgencyUpdatedCampaignContent)
            {
                message = "Doanh nghiệp {0} thêm ghi chú  chiến dịch {1} của bạn";
            }
            else if (type == NotificationType.AccountSubmitCampaignContent)
            {
                message = "Thành viên {0} đã gửi nội dung chiến dịch {1}";
            }

            else if (type == NotificationType.AccountVerifyDenied)
            {
                message = "Thành viên {0} đã bị từ chối xác thực bởi hệ thống {1} vì thông tin chưa hợp lệ";
            }

            else if (type == NotificationType.AccountVerifySuccess)
            {
                message = "Thành viên {0} đã được xác thực bởi hệ thống {1}";
            }

            return string.Format(message, args);
        }
    }
}
