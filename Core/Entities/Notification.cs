﻿using System;
using System.Collections.Generic;
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
        AgencyRequestJoinCampaign,
        AgencyConfirmJoinCampaign,
        AccountRequestJoinCampaign,
        AccountConfirmJoinCampaign,
        CampaignStarted,
        CampaignEnded,
    }
    public enum NotificationStatus
    {
        Created = 0,
        Checked = 1
    }

    public static class NotificationExt
    {
        public static string GetMessageText(this NotificationType type, params string[] args)
        {
            var message = "";
            if (type == NotificationType.AccountConfirmJoinCampaign)
            {
                message = "Thành viên {0} đã đồng ý tham gia chiến dịch {1}";
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
            else if (type == NotificationType.CampaignStarted)
            {
                message = "Chiến dịch {0} đã kết thúc";
            }
            return string.Format(message, args);
        }
    }
}
