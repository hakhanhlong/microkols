using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
    public class NotificationSpecification : BaseSpecification<Notification>
    {
        public NotificationSpecification(EntityType entityType, int entityId, NotificationStatus? status) :

          base(m => m.EntityId == entityId && m.EntityType == entityType && (!status.HasValue || m.Status == status.Value))
        {

        }

        public NotificationSpecification(EntityType entityType, int entityId, NotificationType type, int dataid) :

            base(m => m.Type == type && m.EntityId == entityId && m.EntityType == entityType && m.DataId == dataid)
        {

        }

    }
}
