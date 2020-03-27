using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Goto(int id)
        {
            return View();
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
    }
}