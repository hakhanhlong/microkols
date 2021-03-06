﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebMerchant.Controllers
{
    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        private const int pagesize = 20;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        #region Notification
        public async Task<IActionResult> Count()
        {
            var count = await _notificationService.GetCountNotification(CurrentUser.Type, CurrentUser.Id, Core.Entities.NotificationStatus.Created);
            return Json(count);
        }


        public async Task<IActionResult> Index(NotificationTypeGroup? type,string daterange,int pageindex = 1)
        {
            ViewBag.type = type;
            ViewBag.daterange = daterange;

            ListNotificationViewModel _list = new ListNotificationViewModel();

            if(type == null)
            {
                _list = await _notificationService.GetNotifications(CurrentUser.Type, CurrentUser.Id, type, daterange, string.Empty, pageindex, pagesize);
            }


            if (type == NotificationTypeGroup.System)
            {
                _list = await _notificationService.GetNotificationByGroup(CurrentUser.Type, CurrentUser.Id, type.ToString(), daterange, pageindex, pagesize);
            }


            if (type == NotificationTypeGroup.Campaign)
            {
                _list = await _notificationService.GetNotificationByGroup(CurrentUser.Type, CurrentUser.Id, type.ToString(), daterange, pageindex, pagesize);
            }

            if (type == NotificationTypeGroup.Payment)
            {
                _list = await _notificationService.GetNotificationByGroup(CurrentUser.Type, CurrentUser.Id, type.ToString(), daterange, pageindex, pagesize);
            }

            if (type == NotificationTypeGroup.Influencer)
            {
                _list = await _notificationService.GetNotificationByGroup(CurrentUser.Type, CurrentUser.Id, type.ToString(), daterange, pageindex, pagesize);
            }


            return View(_list);
        }

        public async Task<IActionResult> IndexPartial()
        {
            var model = await _notificationService.GetNotifications(CurrentUser.Type, CurrentUser.Id, null, string.Empty, 1, 10);
            return PartialView(model);
        }
        public async Task<IActionResult> UpdateChecked()
        {
            await _notificationService.UpdateNotificationChecked(CurrentUser.Type, CurrentUser.Id);
            return Json(true);
        }

        #endregion
    }
}