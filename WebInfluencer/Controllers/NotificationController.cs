using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;

namespace WebInfluencer.Controllers
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


        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _notificationService.GetNotifications(CurrentUser.Type, CurrentUser.Id, null, string.Empty, page, pagesize);
            return View(model);
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