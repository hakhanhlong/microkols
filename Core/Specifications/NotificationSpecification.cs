using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Specifications
{
    public class NotificationSpecification : BaseSpecification<Notification>
    {
        public NotificationSpecification(EntityType entityType, int entityId, NotificationStatus? status) :

          base(m => m.EntityId == entityId && m.EntityType == entityType && (!status.HasValue || m.Status == status.Value))
        {

        }


        public NotificationSpecification(EntityType entityType, int entityId, IEnumerable<NotificationType> type, Common.Helpers.DateRange? dateRange) :

        base(m => m.EntityId == entityId && m.EntityType == entityType &&
            type.Contains(m.Type) && (!dateRange.HasValue || (m.DateCreated > dateRange.Value.Start && m.DateCreated < dateRange.Value.End)))
        {

        }
        public NotificationSpecification(EntityType entityType, int entityId, NotificationType type, int dataid) :

            base(m => m.Type == type && m.EntityId == entityId && m.EntityType == entityType && m.DataId == dataid)
        {

        }

    }
}
