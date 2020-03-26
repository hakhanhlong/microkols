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
    }
}