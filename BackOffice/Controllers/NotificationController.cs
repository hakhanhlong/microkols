﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            if (notification.Data == "Influencer" && notification.Type == NotificationType.AccountSendVerify)
            {
                href = "/microkol/verify/?id=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.CampaignCreated)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }

            if (notification.Data == "Campaign" && notification.Type == NotificationType.AgencyPayCampaignService)
            {
                href = "/campaign/detail/?campaignid=" + notification.DataId;
            }




            await _notificationService.UpdateChecked(id);


            return Redirect(href);
        }

        public async Task<IActionResult> AjaxDropNotification()
        {
            var number_notification = await _notificationService.CountNotification(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created);
            ViewBag.NumberNotification = number_notification;

            var list_notification = await _notificationService.GetNewNotifications(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, 1, 50);

            return View(list_notification);
        }

        public async Task<IActionResult> AjaxRecentNotification()
        {            
            var list_notification = await _notificationService.GetNewNotifications(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created, 1, 12);
            return View(list_notification);
        }



        public async Task<JsonResult> CountNewNotification()
        {                       
            int CampaignCount = await _notificationService.CountNotification(Core.Entities.EntityType.System, Core.Entities.NotificationStatus.Created,  new List<NotificationType>() { NotificationType.CampaignCreated,
                NotificationType.AgencyPayCampaignService});
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