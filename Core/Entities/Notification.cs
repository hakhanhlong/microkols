using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Notification: BaseEntity
    {
        public int AccountId { get; set; }
        public NotificationType Type { get; set; }
        public string Data { get; set; }
        public int DataId { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public NotificationStatus Status { get; set; }
    }
    public enum NotificationType
    {

    }
    public enum NotificationStatus
    {
        Created  = 0,
        Checked = 1
    }
}
