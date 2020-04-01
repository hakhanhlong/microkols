using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Code;

namespace WebServices.ViewModels
{

    public class CreateNotificationViewModel
    {
        [Required]
        public string Content { get; set; }
        public int DataId { get; set; }
        public string Data { get; set; }
        public NotificationType Type { get; set; }
    }
    public class ListNotificationViewModel
    {
        public List<NotificationViewModel> Notifications { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class NotificationViewModel
    {
        public NotificationViewModel()
        {

        }


        public NotificationViewModel(Notification notification)
        {
            Id = notification.Id;
            Date = notification.DateCreated;
            Type = notification.Type;
            DataId = notification.DataId;
            Data = notification.Data;
            Status = notification.Status;
            EntityType = notification.EntityType;
            EntityId = notification.EntityId;
            TypeGroup = notification.Type.ToTypeGroup();
            Message = (notification.Type == NotificationType.CampaignCanceled) ? $"{notification.Message} với lý do {notification.Data}" : $"{notification.Message}";
            NotificationTypeToText = notification.Type.NotificationTypeToText();

        }


        public string NotificationLink
        {
            get
            {               

                if (Data == "Transaction" && Type == NotificationType.AgencyWalletDeposit)
                {
                    return "/notification/goto?id=" + Id;
                }

                if (Data == "Transaction" && Type == NotificationType.AgencyWalletWithDraw)
                {
                    return "/notification/goto?id=" + Id;
                }

                return "/notification/goto?id=" + Id;
            }
        }

        public string NotificationTypeToText { get; set; }

        public bool Checked { get; set; } = false;

        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public NotificationTypeGroup TypeGroup
        {
            get;set;
        }
        public int DataId { get; set; }
        public string Data { get; set; }
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }

        public NotificationStatus Status { get; set; }
        public string EntityImage { get; set; }


        public string Message { get; set; }
        public DateTime Date { get; set; }
        //public string DateText
        //{
        //    get
        //    {
        //        return Date.ToViDate();
        //    }
        //}



    }

}
