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

      }
}
