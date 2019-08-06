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
            

            return View(listing);
        }

        public IActionResult Search()
        {
            return View();
        }
    }
}