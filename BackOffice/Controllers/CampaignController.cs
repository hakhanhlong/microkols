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
using WebServices.Services;

namespace BackOffice.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {

        ICampaignBusiness _ICampaignBusiness;
        ICampaignRepository _ICampaignRepository;
        ITransactionRepository _ITransactionRepository;

        IAgencyBusiness _IAgencyBusiness;
        private readonly ISharedBusiness _ISharedBusiness;
        private readonly INotificationBusiness _INotificationBusiness;

        ICampaignService _ICampaignService;
        ITransactionService _TransactionService;


        public CampaignController(ICampaignBusiness __ICampaignBusiness, ICampaignRepository __ICampaignRepository, IAgencyBusiness __IAgencyBusiness, 
            ISharedBusiness __ISharedBusiness, INotificationBusiness __INotificationBusiness, ICampaignService __ICampaignService, 
            ITransactionRepository __ITransactionRepository, ITransactionService __ITransactionService)
        {
            _ICampaignBusiness = __ICampaignBusiness;
            _ICampaignRepository = __ICampaignRepository;
            _IAgencyBusiness = __IAgencyBusiness;
            _ISharedBusiness = __ISharedBusiness;
            _INotificationBusiness = __INotificationBusiness;
            _ICampaignService = __ICampaignService;
            _ITransactionRepository = __ITransactionRepository;
            _TransactionService = __ITransactionService;

        }

        public IActionResult Index(int pageindex = 1)
        {
            var listing = _ICampaignBusiness.GetListCampaign(pageindex, 25);
            DataSelectionStatusAndType();
            return View(listing);
        }

        public async Task<IActionResult> CampaignFollowAgency(int agencyid = 0, int pageindex = 1)
        {
            var listing = await _ICampaignBusiness.GetListCampaignByAgency(agencyid, pageindex, 25);
            var agency = await _IAgencyBusiness.GetAgency(agencyid);
            ViewBag.Agency = agency;
            DataSelectionStatusAndType();
            return View(listing);
        }

        private void DataSelectionStatusAndType()
        {
            ViewBag.CampaignStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "All", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Created", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Confirmed", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Started", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Ended", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Completed", Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Canceled", Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Error", Value = "6"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Locked", Value = "7"},

            };

            ViewBag.CampaignTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "All", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ShareContent", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ShareContentWithCaption", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ChangeAvatar", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "PostComment", Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "JoinEvent", Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ShareStreamUrl", Value = "6"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "CustomService", Value = "7"},
            };
        }

        public IActionResult Search(string kw, CampaignType? type, CampaignStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {

            DataSelectionStatusAndType();
            var list = _ICampaignBusiness.Search(kw, type, status, StartDate, EndDate, pageindex, 25);

            return View(list);
        }

        public async Task<IActionResult> Detail(int agencyid = 0, int campaignid = 0)
        {

            CampaignDetailsViewModel campaign;

            if(agencyid > 0)
            {
                campaign = await _ICampaignBusiness.GetCampaign(agencyid, campaignid);
            }
            else
            {
                campaign = await _ICampaignBusiness.GetCampaign(campaignid);
            }

            
            ViewBag.Categories = await _ISharedBusiness.GetCategories();            

            ViewBag.Cities = await _ISharedBusiness.GetCities();
            return View(campaign);
        }


        public async Task<IActionResult> Configuration(int campaignid = 0)
        {

            WebServices.ViewModels.CampaignViewModel campaign;
            campaign = await _ICampaignService.GetCampaign(campaignid);            
            return View(campaign);
        }

        [HttpPost]
        public async Task<IActionResult> Configuration(WebServices.ViewModels.CampaignViewModel model)
        {

            await _ICampaignService.UpdateCampaignServiceChargePercent(model.ServiceChargePercent, model.Id);
            TempData["MessageSuccess"] = string.Format("Configudation Campaign {0} Success!", model.Title);
            return RedirectToAction("Configuration", "Campaign", new { campaignid = model.Id });
        }


        public async Task<IActionResult> TakeNoteChangeStatus(int id, CampaignStatus status)
        {            
            var campaign = await _ICampaignRepository.GetByIdAsync(id);
            //get info payment from agency fee service on campaign
            //id = campaignid
            var payment = await _TransactionService.GetTransaction(TransactionType.CampaignServiceCharge, id);
            if (payment != null) {
                ViewBag.Payment = payment;
            }
            

            DataSelectionStatusAndType();
            return View(new CampaignViewModel(campaign));
        }

        [HttpPost]
        public async Task<IActionResult> TakeNoteChangeStatus(int id, CampaignStatus status, string txt_note)
        {
            var campaign = await _ICampaignRepository.GetByIdAsync(id);
            DataSelectionStatusAndType();

            try {
                if (campaign != null)
                {

                    campaign.Status = status;
                    campaign.UserModified = HttpContext.User.Identity.Name;
                    campaign.SystemNote = txt_note;
                    _ICampaignRepository.Update(campaign);

                    NotificationType notificationType = NotificationType.CampaignCanceled;
                    string msg = string.Empty;
                    if (status == CampaignStatus.Canceled)
                    {
                        notificationType = NotificationType.CampaignCanceled;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã bị hủy, bởi hệ thống", campaign.Title);
                    }
                    else if (status == CampaignStatus.Error)
                    {
                        notificationType = NotificationType.CampaignError;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã có lỗi, hệ thống đã phát hiện lỗi và gửi thông báo đến bạn", campaign.Title);
                    }
                    else if (status == CampaignStatus.Ended)
                    {
                        notificationType = NotificationType.CampaignEnded;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã kết thúc", campaign.Title);
                    }
                    else if(status == CampaignStatus.Confirmed)
                    {
                        notificationType = NotificationType.CampaignConfirmed;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã được duyệt bởi hệ thống", campaign.Title);
                    }
                    else if (status == CampaignStatus.Completed)
                    {
                        notificationType = NotificationType.CampaignCompleted;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã hoàn thành", campaign.Title);
                    }

                    await _INotificationBusiness.CreateNotificationCampaignByStatus(campaign.Id, campaign.AgencyId, notificationType, msg, txt_note);

                    TempData["MessageSuccess"] = string.Format("Change status \"{0}\" success", status.ToString());


                    //if(status == CampaignStatus.Canceled || status == CampaignStatus.Error || status == CampaignStatus.Ended)
                    //{

                    //}
                    //else
                    //{
                    //    TempData["MessageError"] = "Status campaign do not fit";
                    //}


                }
                else
                {
                    TempData["MessageError"] = "Campaign do not exist!";
                }
            }
            catch(Exception ex) {
                TempData["MessageError"] = ex.Message;
            }




            return RedirectToAction("TakeNoteChangeStatus", "Campaign", new { id = id, status = (int)status });
        }



        [HttpPost]
        public async Task<JsonResult> ChangeStatus(int id, CampaignStatus status)
        {
            var campaign = _ICampaignRepository.GetById(id);
            if(campaign!= null)
            {
                campaign.Status = status;
                campaign.UserModified = HttpContext.User.Identity.Name;
                


                string str_icon = string.Empty;
                NotificationType notificationType = NotificationType.CampaignConfirmed;
                string msg = string.Empty;
                if (status == CampaignStatus.Created)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\">< i class=\"fa fa-sticky-note\"></i></a>";
                }
                else if (status == CampaignStatus.Started)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-success m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-heartbeat\"></i></a>";
                }
                else if (status == CampaignStatus.Ended)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\">< i class=\"fa fa-stop\"></i></a>";
                }
                else if (status == CampaignStatus.Confirmed)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-check-circle-o\"></i></a>";
                    msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã được duyệt bởi hệ thống", campaign.Title);
                    notificationType = NotificationType.CampaignConfirmed;
                    campaign.SystemNote = msg;
                    await _INotificationBusiness.CreateNotificationCampaignByStatus(campaign.Id, campaign.AgencyId, notificationType, msg, "");
                }
                else if (status == CampaignStatus.Error)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-times-circle\"></i></a>";
                }
                else if (status == CampaignStatus.Completed)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-check-circle-o\"></i></a>";
                    msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã hoàn thành", campaign.Title);
                    notificationType = NotificationType.CampaignCompleted;
                    campaign.SystemNote = msg;
                    await _INotificationBusiness.CreateNotificationCampaignByStatus(campaign.Id, campaign.AgencyId, notificationType, msg, "");
                }
                else if (status == CampaignStatus.Canceled)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-warning m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-ban\"></i></a>";
                }

                _ICampaignRepository.Update(campaign);

                return Json(new
                {
                    code = 1,
                    message = "Change campaign status success",
                    str_icon = str_icon
                });
            }

            return Json(new {
                code = -1,
                message = "Error can't change status campaign!",
                str_icon = ""
            });
        }



        public IActionResult Microkol(string kw, CampaignType? type, CampaignStatus? status, int pageindex = 1)
        {

            DataSelectionStatusAndType();
            var list = _ICampaignBusiness.Search(kw, type, status, pageindex, 25);

            return View(list);
        }


    }
}