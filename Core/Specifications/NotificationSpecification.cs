using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class NotificationSpecification : BaseSpecification<Notification>
    {
        public NotificationSpecification(NotificationType type, int accountid, int dataid) : base(m => m.Type == type && m.AccountId == accountid && m.DataId == dataid)
        {

        }
        public NotificationSpecification(int accountid) : base(i => i.AccountId == accountid)
        {

        }

        public NotificationSpecification(int accountid, NotificationStatus status) : base(i => i.AccountId == accountid && i.Status == status)
        {

        }
    }
}
