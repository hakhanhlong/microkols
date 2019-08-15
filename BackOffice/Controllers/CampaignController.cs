using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
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

        public CampaignController(ICampaignBusiness __ICampaignBusiness, ICampaignRepository __ICampaignRepository)
        {
            _ICampaignBusiness = __ICampaignBusiness;
            _ICampaignRepository = __ICampaignRepository;
        }

        public IActionResult Index(int pageindex = 1)
        {

            var listing = _ICampaignBusiness.GetListCampaign(pageindex, 25);

            ViewBag.CampaignStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Created", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Confirmed", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Started", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Ended", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Completed", Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Canceled", Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Error", Value = "6"},
            };
            return View(listing);
        }

        public IActionResult Search()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int agencyid = 0, int campaignid = 0)
        {

            var campaign = await _ICampaignBusiness.GetCampaign(agencyid, campaignid);


            return View(campaign);
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
    }
}