using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;

namespace BackOffice.Controllers
{

    [Authorize]
    public class AgencyController : Controller
    {        
        private readonly IAgencyBusiness _IAgencyBusiness;        
        INotificationBusiness _INotificationBusiness;
        INotificationService _INotificationService;

        public AgencyController(IAgencyBusiness __IAgencyBusiness, INotificationBusiness __INotificationBusiness, INotificationService __INotificationService)
        {
            _IAgencyBusiness = __IAgencyBusiness;
            _INotificationBusiness = __INotificationBusiness;
            _INotificationService = __INotificationService;
        }

        public IActionResult Index(int pageindex = 1)
        {
            var list_agency = _IAgencyBusiness.GetListAgency(pageindex, 20);
            return View(list_agency);
        }


        public async Task<IActionResult> SendNotification(int id = 1)
        {
            var agency = await _IAgencyBusiness.GetAgency(id);                        
            if (agency == null)
            {
                TempData["MessageError"] = "Doanh nghiệp không tồn tại!";
            }
            return View(agency);
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification(AgencyViewModel model, string txt_note = "")
        {
            var agency = await _IAgencyBusiness.GetAgency(model.Id);
            if (agency == null)
            {
                TempData["MessageError"] = "Doanh nghiệp không tồn tại!";
            }
            else
            {
                await _INotificationBusiness.CreateNotification(EntityType.Agency, model.Id, model.Id,
                    NotificationType.SystemSendNotifycation, txt_note, "");


                TempData["MessageSuccess"] = "Đã gửi thông báo thành công!";
            }
            return RedirectToAction("SendNotification", "Agency", new { id = model.Id });

        }

        public IActionResult Search(string keyword, int pageindex = 1)
        {
            var list_agency = _IAgencyBusiness.Search(keyword, pageindex, 25);
            return View(list_agency);
        }

        public async Task<IActionResult> Detail(int id = 0)
        {
            var agency = await _IAgencyBusiness.GetAgency(id);
            return View(agency);
        }

        [HttpPost]
        public async Task<IActionResult> Detail(AgencyViewModel model)
        {
            await _IAgencyBusiness.UpdateAgency(model);
            TempData["MessageSuccess"] = "Update thành công!";
            return Redirect("/agency/detail/?id=" + model.Id);
        }


        public IActionResult Active(int id)
        {
            
            if (_IAgencyBusiness.Active(id))
            {
                TempData["MessageSuccess"] = "Active Agency Success!";
            }
            else
            {
                TempData["MessageError"] = "Active Agency Error!";
            }


            return RedirectToAction("index", "agency");
        }


        public IActionResult UnActive(int id)
        {
            
            if (_IAgencyBusiness.UnActive(id))
            {
                TempData["MessageSuccess"] = "UnActive Agency Success!";
            }
            else
            {
                TempData["MessageError"] = "UnActive Agency Error!";
            }

            return RedirectToAction("index", "agency");

        }


    }


}