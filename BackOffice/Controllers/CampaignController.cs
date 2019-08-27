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

namespace BackOffice.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {

        ICampaignBusiness _ICampaignBusiness;
        ICampaignRepository _ICampaignRepository;
        IAgencyBusiness _IAgencyBusiness;
        private readonly ISharedBusiness _ISharedBusiness;
        private readonly INotificationBusiness _INotificationBusiness;

        public CampaignController(ICampaignBusiness __ICampaignBusiness, ICampaignRepository __ICampaignRepository, IAgencyBusiness __IAgencyBusiness, 
            ISharedBusiness __ISharedBusiness, INotificationBusiness __INotificationBusiness)
        {
            _ICampaignBusiness = __ICampaignBusiness;
            _ICampaignRepository = __ICampaignRepository;
            _IAgencyBusiness = __IAgencyBusiness;
            _ISharedBusiness = __ISharedBusiness;
            _INotificationBusiness = __INotificationBusiness;
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
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "All", Value = "-1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Created", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Confirmed", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Started", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Ended", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Completed", Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Canceled", Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Error", Value = "6"},
            };

            ViewBag.CampaignTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "All", Value = "-1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ShareContent", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ShareContentWithCaption", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ChangeAvatar", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "PostComment", Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "JoinEvent", Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "ShareStreamUrl", Value = "6"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "CustomService", Value = "7"},
            };
        }

        public IActionResult Search(string kw, CampaignType? type, CampaignStatus? status, int pageindex = 1)
        {

            DataSelectionStatusAndType();
            var list = _ICampaignBusiness.Search(kw, type, status, pageindex, 25);

            return View(list);
        }

        public async Task<IActionResult> Detail(int agencyid = 0, int campaignid = 0)
        {

            var campaign = await _ICampaignBusiness.GetCampaign(agencyid, campaignid);
            ViewBag.Categories = await _ISharedBusiness.GetCategories();            
            ViewBag.Cities = await _ISharedBusiness.GetCities();
            return View(campaign);
        }

        public async Task<IActionResult> TakeNoteChangeStatus(int id, CampaignStatus status)
        {            
            var campaign = await _ICampaignRepository.GetByIdAsync(id);
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
                    _ICampaignRepository.Update(campaign);

                    NotificationType notificationType = NotificationType.CampaignCanceled;

                    if (status == CampaignStatus.Canceled)
                    {
                        notificationType = NotificationType.CampaignCanceled;
                    }
                    else if(status == CampaignStatus.Error)
                    {
                        notificationType = NotificationType.CampaignError;
                    }
                    else if(status == CampaignStatus.Ended)
                    {
                        notificationType = NotificationType.CampaignEnded;
                    }


                }
                else
                {
                    TempData["MessageError"] = "Campaign do not exist!";
                }
            }
            catch(Exception ex) {
                TempData["MessageError"] = ex.Message;
            }




            return View(new CampaignViewModel(campaign));
        }



        [HttpPost]
        public JsonResult ChangeStatus(int id, CampaignStatus status)
        {
            var campaign = _ICampaignRepository.GetById(id);
            if(campaign!= null)
            {
                campaign.Status = status;
                campaign.UserModified = HttpContext.User.Identity.Name;
                _ICampaignRepository.Update(campaign);


                string str_icon = string.Empty;
                if(status == CampaignStatus.Created)
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
                }
                else if (status == CampaignStatus.Error)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-times-circle\"></i></a>";
                }
                else if (status == CampaignStatus.Completed)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-check-circle-o\"></i></a>";
                }
                else if (status == CampaignStatus.Canceled)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-warning m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-ban\"></i></a>";
                }

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