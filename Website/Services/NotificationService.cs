
using Website.Code;
using Website.Interfaces;
using Website.ViewModels;
using Common.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Website.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IAsyncRepository<Notification> _notificationRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemoryCache _cache;

        public NotificationService(ILoggerFactory loggerFactory, IMemoryCache cache,ICampaignRepository campaignRepository,
            IAccountRepository accountRepository,
             IAsyncRepository<Notification> notificationRepository)
        {
            _logger = loggerFactory.CreateLogger<NotificationService>();
            _notificationRepository = notificationRepository;
            _cache = cache;
            _campaignRepository = campaignRepository;
            _accountRepository =accountRepository;
        }

        #region Notification

        public async Task<int> GetCountNotification(EntityType entityType,int entityId, NotificationStatus? status)
        {
            return await _notificationRepository.CountAsync(new NotificationSpecification(entityType, entityId, status));
        }

        public async Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, int entityId)
        {
            var notifications = await _notificationRepository.ListAsync(new NotificationSpecification(entityType, entityId, NotificationStatus.Created));
            if (notifications.Count == 0)
            {
                return await GetNotifications(entityType, entityId, null, "", 1, 10);
            }

            return await GetNotifications(notifications.Take(10));
        }

        public async Task<List<NotificationViewModel>> GetNotifications(IEnumerable<Notification> notifications)
        {
            var result = new List<NotificationViewModel>();
            foreach (var item in notifications)
            {
                EntityType entityType = EntityType.Agency;
                int entityid = 0;
                string entityImage = string.Empty;
                string entityText = string.Empty;


                /*
                if (item.Type == NotificationType.ca)
                {
                    var campaign = await _campaignRepository.GetByIdAsync(item.DataId);
                    if (campaign != null)
                    {
                        entityType = EntityType.Campaign;
                        entityid = campaign.Id; 
                        entityImage = string.Empty;
                        entityText = campaign.Code;
                    }
                }
                */
                var notif = new NotificationViewModel(item);
                notif.EntityId = entityid;
                notif.EntityText = entityText;
                notif.EntityType = entityType;
                notif.EntityImage = entityImage;

                result.Add(notif);
            }

            return result;
        }

        public async Task<List<NotificationViewModel>> GetNotifications(EntityType entityType, int entityId, NotificationStatus? status, string campaign, int page, int pagesize)
        {
            var notifications = await _notificationRepository.ListPagedAsync(new NotificationSpecification(entityType, entityId, status),
                campaign, page, pagesize);

            return await GetNotifications(notifications);
        }


        public async Task<List<NotificationViewModel>> GetNotifications(EntityType entityType, int entityId, NotificationStatus? status)
        {
            var notifications = await _notificationRepository.ListAsync(new NotificationSpecification(entityType, entityId, status));
            return await GetNotifications(notifications);
        }


        public async Task UpdateNotificationChecked(EntityType entityType, int entityId)
        {
            var notifications = await _notificationRepository.ListAsync(new NotificationSpecification(entityType, entityId,
                NotificationStatus.Created));

            foreach (var notif in notifications)
            {
                notif.Status = NotificationStatus.Checked;
                await _notificationRepository.UpdateAsync(notif);
            }

        }
        public async Task<int> CreateNotification(EntityType entityType, int entityId,
            CreateNotificationViewModel model, string username)
        {
            var notification = new Notification()
            {

                EntityId = entityId,
                EntityType = entityType,
                DateCreated = DateTime.Now,
                Data = string.Empty,
                DataId = model.DataId,
                Status = NotificationStatus.Created,
                Type = model.Type,
            };

            await _notificationRepository.AddAsync(notification);
            return notification.Id;
        }




        #endregion


    }
}
