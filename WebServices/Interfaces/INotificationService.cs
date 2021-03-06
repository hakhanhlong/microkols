﻿using WebServices.ViewModels;
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

        Task<ListNotificationViewModel> GetNotifications(EntityType entityType, int entityId, NotificationTypeGroup? typeGroup, string daterange,
            string order, int page, int pagesize);


        //######## longhk add #######################################################
        Task<Notification> GetNotification(int notificationid);

        Task<ListNotificationViewModel> GetNotificationByGroup(EntityType entityType, int entityId, string groupName, string daterange, int pageindex, int pagesize);

        Task<int> CountNotification(EntityType entityType, NotificationStatus? status, NotificationType type);

        Task<int> CountNotification(EntityType entityType, NotificationStatus? status, List<NotificationType> type);

        Task<int> CountNotification(EntityType entityType, NotificationStatus? status);

        //Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, NotificationType type, NotificationStatus? status , string data);

        Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, NotificationType type, NotificationStatus? status, int pageindex, int pagesize);

        Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType,  NotificationStatus? status, int pageindex, int pagesize);

        Task<ListNotificationViewModel> GetNotifications(EntityType entityType, int pageindex, int pagesize);


        Task<ListNotificationViewModel> GetNotificationByGroup(EntityType entityType, string groupName, int pageindex, int pagesize);
        Task<ListNotificationViewModel> GetNotificationByGroup(EntityType entityType, NotificationStatus status, string groupName, int pageindex, int pagesize);





        Task<int> UpdateChecked(int id);



        //############################################################################


        Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, int entityId); 
        Task<List<NotificationViewModel>> GetNotifications(EntityType entityType, int entityId, NotificationStatus? status, string order, int page, int pagesize);
        Task<int> GetCountNotification(EntityType entityType, int entityId, NotificationStatus? status);

        Task CreateNotification(int dataid, EntityType entityType, int entityid, NotificationType notificationType, string msg, string text);

        Task CreateNotificationCampaignStarted(int campaignid);
        Task CreateNotificationCampaignEnded(int campaignid);
        Task CreateNotificationCampaignCompleted(int campaignid);
    }
}
