
using WebServices.Code;
using WebServices.Interfaces;
using WebServices.ViewModels;
using Common.Extensions;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Core.Extensions;

namespace WebServices.Services
{
    public class NotificationService : INotificationService
    {

        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMemoryCache _cache;
        private readonly IAsyncRepository<CampaignAccount> _campaignAccountRepository;

        private readonly List<NotificationType> GroupCampaign;
        private readonly List<NotificationType> GroupPayment;
        private readonly List<NotificationType> GroupInfluencer;
        private readonly List<NotificationType> GroupSystem;


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


            GroupCampaign = new List<NotificationType>()
            {
                NotificationType.CampaignStarted,
                NotificationType.CampaignCantStarted,
                NotificationType.CampaignEnded,
                NotificationType.CampaignCompleted,
                NotificationType.CampaignCanceled,
                NotificationType.CampaignConfirmed,
                NotificationType.CampaignError,
                NotificationType.CampaignCreated,
                NotificationType.CampaignLocked,
                NotificationType.AccountSubmitCampaignRefContent,
                NotificationType.AccountFinishCampaignRefContent,
                NotificationType.AgencyApproveCampaignRefContent,
                NotificationType.AgencyDeclineCampaignRefContent,
                NotificationType.AgencyUpdatedCampaignRefContent,
                NotificationType.AgencyCancelAccountJoinCampaign
            };

            GroupPayment = new List<NotificationType>()
            {
                NotificationType.AgencyWalletDeposit,
                NotificationType.AgencyWalletWithDraw,
                NotificationType.AgencyPayCampaignService,
                NotificationType.AgencyRequestWithdrawFromCampaign,
                NotificationType.ExcecutedPaymentToAccountBanking
            };

            GroupInfluencer = new List<NotificationType>()
            {
                NotificationType.AccountSendVerify,
                NotificationType.AccountRequestJoinCampaign,
                NotificationType.AccountConfirmJoinCampaign,
                NotificationType.AccountDeclineJoinCampaign,                
                NotificationType.AccountVerifyDenied,
                NotificationType.AccountVerifySuccess,
            };

            GroupSystem = new List<NotificationType>()
            {
                
            };




        }


        #region Notification



        #region longhk definition

        //############################# longhk add ########################################################################
        public async Task<Notification> GetNotification(int notificationid)
        {
            return await _notificationRepository.GetByIdAsync(notificationid);
        }



        public async Task<int> CountNotification(EntityType entityType, NotificationStatus? status, NotificationType type)
        {
            var notifications = await _notificationRepository.CountAsync(new NotificationSpecification(entityType, status.Value , type));
            return notifications;
        }

        public async Task<int> CountNotification(EntityType entityType, NotificationStatus? status, List<NotificationType> type)
        {
            var notifications = await _notificationRepository.CountAsync(new NotificationSpecification(entityType, status.Value, type));
            return notifications;
        }

        public async Task<int> CountNotification(EntityType entityType, NotificationStatus? status)
        {
            var notifications = await _notificationRepository.CountAsync(new NotificationSpecification(entityType, status.Value));
            return notifications;
        }

        public async Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, NotificationType type, NotificationStatus? status, int pageindex, int pagesize)
        {
            var notifications = await _notificationRepository.ListPagedAsync(new NotificationSpecification(entityType, status.Value, type),
                "DateCreated_desc", pageindex, pagesize);

            return await GetNotifications(notifications);
        }

        public async Task<List<NotificationViewModel>> GetNewNotifications(EntityType entityType, NotificationStatus? status, int pageindex, int pagesize)
        {
            var notifications = await _notificationRepository.ListPagedAsync(new NotificationSpecification(entityType, status.Value),
                "DateCreated_desc", pageindex, pagesize);


            return await GetNotifications(notifications);
        }

        public async Task<ListNotificationViewModel> GetNotifications(EntityType entityType, int pageindex, int pagesize)
        {

            var filter = new NotificationSpecification(entityType);

            var notifications = await _notificationRepository.ListPagedAsync(filter, "DateCreated_desc", pageindex, pagesize);

            var total = await _notificationRepository.CountAsync(filter);

            var list = await GetNotifications(notifications);
            return new ListNotificationViewModel()
            {
                Notifications = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }


        public async Task<int> UpdateChecked(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            notification.Status = NotificationStatus.Checked;
            await _notificationRepository.UpdateAsync(notification);

            if (notification.Id > 0)
                return notification.Id;

            return 0;

        }


        public async Task<ListNotificationViewModel> GetNotificationByGroup(EntityType entityType, string groupName, int pageindex, int pagesize)
        {
            List<NotificationType> _list_notification_type = new List<NotificationType>();
            if (groupName == "Campaign")
                _list_notification_type = GroupCampaign;
            else if(groupName == "Payment")
                _list_notification_type = GroupPayment;
            else if (groupName == "Influencer")
                _list_notification_type = GroupInfluencer;

            var filter = new NotificationSpecification(entityType, _list_notification_type);

            var notifications = await _notificationRepository.ListPagedAsync(filter, "DateCreated_desc", pageindex, pagesize);
            var total = await _notificationRepository.CountAsync(filter);
            var list = await GetNotifications(notifications);
            return new ListNotificationViewModel()
            {
                Notifications = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };


        }
        public async Task<ListNotificationViewModel> GetNotificationByGroup(EntityType entityType, NotificationStatus status, string groupName, int pageindex, int pagesize)
        {
            List<NotificationType> _list_notification_type = new List<NotificationType>();
            if (groupName == "Campaign")
                _list_notification_type = GroupCampaign;
            else if (groupName == "Payment")
                _list_notification_type = GroupPayment;
            else if (groupName == "Influencer")
                _list_notification_type = GroupInfluencer;

            var filter = new NotificationSpecification(entityType, status, _list_notification_type);

            var notifications = await _notificationRepository.ListPagedAsync(filter, "DateCreated_desc", pageindex, pagesize);
            var total = await _notificationRepository.CountAsync(filter);
            var list = await GetNotifications(notifications);
            return new ListNotificationViewModel()
            {
                Notifications = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }



        //#################################################################################################################

        #endregion

        public async Task CreateNotification(int dataid, EntityType entityType, int entityid, NotificationType notificationType, string msg, string text)
        {

            var notify = await _notificationRepository.GetSingleBySpecAsync(new NotificationSpecification(entityType, entityid, notificationType, dataid, NotificationStatus.Created));
            if(notify == null)
            {
                Notification _notification = new Notification();
                _notification.EntityType = entityType;
                _notification.EntityId = entityid;
                _notification.DataId = dataid;
                _notification.Message = msg;
                _notification.DateCreated = DateTime.Now;
                _notification.Status = NotificationStatus.Created;
                _notification.Type = notificationType;
                _notification.Data = text;
                await _notificationRepository.AddAsync(_notification);
            }

            

        }

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

        public async Task<List<NotificationViewModel>> GetNotifications(EntityType entityType, int entityId, NotificationStatus? status, string order, int page, int pagesize)
        {
            var notifications = await _notificationRepository.ListPagedAsync(new NotificationSpecification(entityType, entityId, status),
                order, page, pagesize);

            return await GetNotifications(notifications);
        }



        public async Task<ListNotificationViewModel> GetNotifications(EntityType entityType, int entityId, NotificationTypeGroup? typeGroup,string daterange,
            string order, int page, int pagesize)
        {
            
            var statusArr = typeGroup.GetNotificationTypes();
            var dtRange = Common.Helpers.DateRangeHelper.GetDateRange(daterange);
            var filter = new NotificationSpecification(entityType, entityId, statusArr, dtRange);

            var notifications = await _notificationRepository.ListPagedAsync(filter,order, page, pagesize);
            var total = await _notificationRepository.CountAsync(filter);

            var list = await GetNotifications(notifications);
            return new ListNotificationViewModel()
            {
                Notifications = list,
                Pager = new PagerViewModel(page, pagesize, total)
            };
        }

        public async Task<ListNotificationViewModel> GetNotificationByGroup(EntityType entityType, int entityId, string groupName, string daterange, int pageindex, int pagesize)
        {
            List<NotificationType> _list_notification_type = new List<NotificationType>();
            if (groupName == "Campaign")
                _list_notification_type = GroupCampaign;
            else if (groupName == "Payment")
                _list_notification_type = GroupPayment;
            else if (groupName == "Influencer")
                _list_notification_type = GroupInfluencer;

            var dtRange = Common.Helpers.DateRangeHelper.GetDateRange(daterange);
            var filter = new NotificationSpecification(entityType, entityId , _list_notification_type.AsEnumerable(), dtRange);

            var notifications = await _notificationRepository.ListPagedAsync(filter, "DateCreated_desc", pageindex, pagesize);
            var total = await _notificationRepository.CountAsync(filter);
            var list = await GetNotifications(notifications);
            return new ListNotificationViewModel()
            {
                Notifications = list,
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };


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

            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if(campaign!= null)
            {
                var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, CampaignAccountStatus.Finished));

                foreach (var campaignAccount in campaignAccounts)
                {
                    long amount = campaign.GetAccountChagreAmount(campaignAccount);

                    await _notificationRepository.CreateNotification(NotificationType.CampaignCompleted, EntityType.Account, campaignAccount.AccountId, campaignid,
                   NotificationType.CampaignCompleted.GetMessageText(campaign.Title.ToString(), amount.ToPriceText()));
                }
            }
          
        }

        public async Task CreateNotificationCampaignStarted(int campaignid)
        {

            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, null, new List<CampaignAccountStatus>(){
                CampaignAccountStatus.Canceled, CampaignAccountStatus.AccountRequest, CampaignAccountStatus.AgencyRequest
            }));

            foreach (var campaignAccount in campaignAccounts)
            {
                var campaign = await _campaignRepository.GetByIdAsync(campaignid);
                await _notificationRepository.CreateNotification(NotificationType.CampaignStarted, EntityType.Account, campaignAccount.AccountId, campaignid,
               NotificationType.CampaignStarted.GetMessageText(campaign.Title.ToString()));
            }
        }

        public async Task CreateNotificationCampaignEnded(int campaignid)
        {

            //var campaignAccounts = await _campaignAccountRepository.ListAsync(new ConfirmedCampaignAccountSpecification(campaignid));
            var campaignAccounts = await _campaignAccountRepository.ListAsync(new CampaignAccountSpecification(campaignid, new List<CampaignAccountStatus>(){
                CampaignAccountStatus.Finished
            }, null));
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            foreach (var campaignAccount in campaignAccounts)
            {
                await _notificationRepository.CreateNotification(NotificationType.CampaignEnded, EntityType.Account, campaignAccount.AccountId, campaignid,
                NotificationType.CampaignEnded.GetMessageText(campaign.Title.ToString()));
            }
        }
        #endregion
    }
}
