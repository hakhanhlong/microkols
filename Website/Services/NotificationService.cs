
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
        private readonly INotificationRepository _notificationRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemoryCache _cache;
        private readonly IAsyncRepository<CampaignAccount> _campaignAccountRepository;


        public NotificationService(ILoggerFactory loggerFactory, IMemoryCache cache, ICampaignRepository campaignRepository,
            IAccountRepository accountRepository,
             INotificationRepository notificationRepository, IAsyncRepository<CampaignAccount> campaignAccountRepository)
        {
            _logger = loggerFactory.CreateLogger<NotificationService>();
            _notificationRepository = notificationRepository;
            _cache = cache;
            _campaignRepository = campaignRepository;
            _accountRepository = accountRepository;
            _campaignAccountRepository = campaignAccountRepository;
        }


        #region Notification

        public async Task<int> GetCountNotification(EntityType entityType, int entityId, NotificationStatus? status)
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
                //EntityType entityType = EntityType.Agency;
                //int entityid = 0;
                //string entityImage = string.Empty;
                //string entityText = string.Empty;


                //if (item.Type == NotificationType.AccountConfirmJoinCampaign  ||
                //    item.Type == NotificationType.AccountRequestJoinCampaign ||
                //    item.Type == NotificationType.AgencyConfirmJoinCampaign ||
                //    item.Type == NotificationType.AgencyRequestJoinCampaign )
                //{
                //    var campaign = await _campaignRepository.GetByIdAsync(item.DataId);
                //    if (campaign != null)
                //    {
                //        entityType = EntityType.Campaign;
                //        entityid = campaign.Id; 
                //        entityImage = string.Empty;
                //        entityText = campaign.Code;
                //    }
                //}

                var notif = new NotificationViewModel(item);
                //notif.EntityId = entityid;
                //notif.EntityText = entityText;
                //notif.EntityType = entityType;
                //notif.EntityImage = entityImage;

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


        #endregion



        #region  Notification Job
        public async Task CreateNotificationCampaignCompleted(int campaignid)
        {

            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, CampaignAccountStatus.Finished));

            foreach (var campaignAccount in campaignAccounts)
            {
                await _notificationRepository.CreateNotification(NotificationType.CampaignCompleted, EntityType.Account, campaignAccount.AccountId, campaignid,
               NotificationType.CampaignCompleted.GetMessageText(campaignid.ToString(), campaignAccount.AccountChargeAmount.ToPriceText()));
            }
        }

        public async Task CreateNotificationCampaignStarted(int campaignid)
        {

            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, null, new List<CampaignAccountStatus>(){
                CampaignAccountStatus.Canceled, CampaignAccountStatus.AccountRequest, CampaignAccountStatus.AgencyRequest
            }));

            foreach (var campaignAccount in campaignAccounts)
            {
                await _notificationRepository.CreateNotification(NotificationType.CampaignStarted, EntityType.Account, campaignAccount.AccountId, campaignid,
               NotificationType.CampaignStarted.GetMessageText(campaignid.ToString()));
            }
        }

        public async Task CreateNotificationCampaignEnded(int campaignid)
        {

            //var campaignAccounts = await _campaignAccountRepository.ListAsync(new ConfirmedCampaignAccountSpecification(campaignid));
            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, new List<CampaignAccountStatus>(){
                CampaignAccountStatus.Finished
            }, null));
            foreach (var campaignAccount in campaignAccounts)
            {
                await _notificationRepository.CreateNotification(NotificationType.CampaignEnded, EntityType.Account, campaignAccount.AccountId, campaignid,
                NotificationType.CampaignEnded.GetMessageText(campaignid.ToString()));
            }
        }
        #endregion
    }
}
