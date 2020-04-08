using Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>, IAsyncRepository<Notification>
    {

        Task<int> CreateNotification(NotificationType type, EntityType entityType, int entityId, int dataid, string message, string data = "", string image = "");

        Task<int> CreateNotification(int dataid,  EntityType entityType, int entityId, NotificationType type, string message, string data = "");

    }
}
