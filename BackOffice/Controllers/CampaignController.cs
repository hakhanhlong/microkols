using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {

        ICampaignBusiness _ICampaignBusiness;

        public CampaignController(ICampaignBusiness __ICampaignBusiness)
        {
            _ICampaignBusiness = __ICampaignBusiness;
        }

        public IActionResult Index(int pageindex = 1)
        {

            var listing = _ICampaignBusiness.GetListCampaign(pageindex, 25);

            ViewBag.CampaignStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Created", Value = "1"},
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
    }
}