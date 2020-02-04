using WebServices.ViewModels;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.Interfaces
{
    public interface INotificationService
    {
        Task UpdateNotificationChecked(EntityType entityType, int entityId);

        Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, int entityId);
        Task<List<NotificationViewModel>> GetNotifications(EntityType entityType, int entityId, NotificationStatus? status);
        Task<List<NotificationViewModel>> GetNotifications(EntityType entityType, int entityId, NotificationStatus? status, string order, int page, int pagesize);
        Task<int> GetCountNotification(EntityType entityType, int entityId, NotificationStatus? status);



        Task CreateNotificationCampaignStarted(int campaignid);
        Task CreateNotificationCampaignEnded(int campaignid);
        Task CreateNotificationCampaignCompleted(int campaignid);
    }
}
