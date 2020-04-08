using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class NotificationRepository : EfRepository<Notification>, INotificationRepository
    {

        public NotificationRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<int> CreateNotification(NotificationType type, EntityType entityType, int entityId, int dataid, string message, string data = "", string image = "")
        {

            var notification = await _dbContext.Notification.FirstOrDefaultAsync(m => m.Type == type && m.EntityType == entityType && m.EntityId == entityId && m.DataId == dataid);
            if (notification == null)
            {
                notification = new Notification()
                {
                    Type = type,
                    EntityType = entityType,
                    EntityId = entityId,
                    DataId = dataid,
                    Data = data,
                    Image = image,
                    Status = NotificationStatus.Created,
                    DateCreated = DateTime.Now,
                    Message = message
                };
                await _dbContext.Notification.AddAsync(notification);
                await _dbContext.SaveChangesAsync();

            }
            else
            {
                notification.DateCreated = DateTime.Now;
                notification.Status = NotificationStatus.Created;
                notification.Message = message;
                await _dbContext.SaveChangesAsync();
            }


            return notification.Id;
        }
    }
}
