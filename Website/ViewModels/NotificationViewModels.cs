using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Website.Code;
using Website.Code.Extensions;

namespace Website.ViewModels
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
            Message  = (notification.Type== NotificationType.CampaignCanceled) ? $"{notification.Message} với lý do {notification.Data}" : $"{notification.Message}"  ;

        }



        public int Id { get; set; }
        public NotificationType Type { get; set; }
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
