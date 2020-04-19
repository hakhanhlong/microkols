using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace BackOffice.Models.Wrap
{
    public class WrapNotification
    {
        public List<GroupNotification> GroupNotifications { get; set; }
    }


    public class GroupNotification
    {
        public string GroupName { get; set; } = string.Empty;
        public string GroupName2 { get; set; } = string.Empty;

        public ListNotificationViewModel List_Notification { get; set; }


    }


}
