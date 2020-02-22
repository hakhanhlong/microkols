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

        TransactionDepositeApprove = 12, 
        TransactionDepositeProcessing = 13,
        TransactionDepositeCancel = 14,
        CampaignStarted  = 15,
        CampaignCantStarted = 16,
        CampaignEnded = 17,
        CampaignCompleted = 18,
        CampaignCanceled = 19,
        CampaignConfirmed = 20,
        CampaignError = 21,
        ExcecutedPaymentToAccountBanking = 22,


        AccountSubmitCampaignCaption = 30,
        AgencyApproveCampaignCaption = 31,
        AgencyDeclineCampaignCaption = 32,
        AgencyUpdatedCampaignCaption = 33,


        AccountSubmitCampaignContent = 40,
        AgencyApproveCampaignContent = 41,
        AgencyDeclineCampaignContent = 42,
        AgencyUpdatedCampaignContent = 43,

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
                message = "{0} không cập nhật được trạng thái bắt đầu  của Chiến dịch {1} vì lý do: {2}";
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


            return string.Format(message, args);
        }
    }
}
