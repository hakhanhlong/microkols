using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Notification : BaseEntity
    {
        public DataType DataType { get; set; }
        public int DataId { get; set; }
        public NotificationType Type { get; set; }
        public string RefData { get; set; }
        public int RefId { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime DateCreated { get; set; }
        public NotificationStatus Status { get; set; }
    }
    public enum NotificationType
    {

    }
    public enum NotificationStatus
    {
        Created = 0,
        Checked = 1
    }
}
