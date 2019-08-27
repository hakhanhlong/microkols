using BackOffice.Business.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class NotificationBusiness: INotificationBusiness
    {
        private readonly ILogger<NotificationBusiness> _logger;
        private readonly INotificationRepository _INotificationRepository;

        public NotificationBusiness(ILoggerFactory loggerFactory, INotificationRepository __INotificationRepository)
        {
            _logger = loggerFactory.CreateLogger<NotificationBusiness>();
            _INotificationRepository = __INotificationRepository;
        }

        public async Task CreateNotificationCampaignByStatus(int campaignid, int entityid, NotificationType notificationType, string msg, string text)
        {
            
            Notification _notification = new Notification();
            _notification.EntityType = EntityType.Agency;
            _notification.EntityId = entityid;
            _notification.DataId = campaignid;
            _notification.Message = msg;
            _notification.DateCreated = DateTime.Now;
            _notification.Status = NotificationStatus.Created;
            _notification.Type = notificationType;
            _notification.Data = text;
            await _INotificationRepository.AddAsync(_notification);

        }

    }
}
