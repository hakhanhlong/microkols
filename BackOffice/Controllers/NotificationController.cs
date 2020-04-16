using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Models.Wrap;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;

namespace BackOffice.Controllers
{
    public class NotificationController : Controller
    {

        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService __NotificationService)
        {
            _notificationService = __NotificationService;
        }

        public async Task<IActionResult> Index(int pageindex = 1, int pagesize = 25)
        {

            var list_notification = await _notificationService.GetNotifications(Core.Entities.EntityType.System, pageindex, pagesize);


            return View(list_notification);
        }

        public async Task<IActionResult> Goto(int id)
        {
            var notification = await _notificationService.GetNotification(id);
            string href = string.Empty;      

            if (notification.Data == "Transaction" && notification.Type == NotificationType.AgencyWalletDeposit)
            {
                href = "/transaction/detail?id=" + notification.DataId;
            }

            if (notification.Data == "Transaction" && notification.Type == NotificationType.AgencyWalletWithDraw)
            {
                href = "/transaction/detail?id=" + notification.DataId;
            }

            if (notification.Data == "Transaction" && notification.Type == NotificationType.AgencyRequestWithdrawFromCampaign)
            {
                href = "/transaction/detail?id=" + notification.DataId;
            }

            


            if (notification.Data == "Influencer" && notification.Type == NotificationType.AccountSendVerify)
            {
                href = "/microkol/verify/?id=" + notification.DataId;
            }



            #region Redirect Campaign

            if (notification.Data == "Campaign" && notification.Type == NotificationType.CampaignCreated)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.CampaignLocked)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.CampaignCanceled)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.CampaignEnded)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.CampaignStarted)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.AgencyPayCampaignService)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            #endregion



            await _notificationService.UpdateChecked(id);


            return Redirect(href);
        }

        public async Task<IActionResult> AjaxDropNotification()
        {
            var number_notification = await _notificationService.CountNotification(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created);
            ViewBag.NumberNotification = number_notification;

            var list_campaign_notification = await _notificationService.GetNotificationByGroup(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, "Campaign", 1, 50);
            var list_payment_notification = await _notificationService.GetNotificationByGroup(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, "Payment", 1, 50);

            var wrap_notification = new WrapNotification();
            List<GroupNotification> _list_group = new List<GroupNotification>()
            {
                new GroupNotification(){GroupName = "Campaign", GroupName2 = "Chiến dịch", List_Notification = list_campaign_notification},
                new GroupNotification(){GroupName = "Payment", GroupName2 = "Thanh toán" , List_Notification = list_payment_notification}
            };
            wrap_notification.GroupNotifications = _list_group;

            return View(wrap_notification);
        }

        public async Task<IActionResult> AjaxRecentNotification()
        {            
            var list_notification = await _notificationService.GetNewNotifications(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, 1, 12);
            return View(list_notification);
        }



        public async Task<JsonResult> CountNewNotification()
        {                       
            int CampaignCount = await _notificationService.CountNotification(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created,  new List<NotificationType>() { NotificationType.CampaignCreated,
                NotificationType.AgencyPayCampaignService, NotificationType.CampaignCanceled, NotificationType.CampaignEnded, NotificationType.CampaignLocked, NotificationType.CampaignStarted});

            int WalletDeposite = await _notificationService.CountNotification(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, new List<NotificationType>() { NotificationType.AgencyWalletDeposit});


            int AccountCount = await _notificationService.CountNotification(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, new List<NotificationType>() { NotificationType.AccountSendVerify });


            return Json(new {
                CampaignTotal = CampaignCount,
                WalletDepositeTotal = WalletDeposite,
                AccountTotal = AccountCount
            });
        }

    }
}