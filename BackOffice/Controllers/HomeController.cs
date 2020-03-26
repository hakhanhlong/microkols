using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using BackOffice.Business.Interfaces;

namespace BackOffice.Controllers
{


    [Authorize]
    public class HomeController : Controller
    {
        ICampaignBusiness _ICampaignBusiness;

        public HomeController(ICampaignBusiness __ICampaignBusiness) {
            _ICampaignBusiness = __ICampaignBusiness;
        }


        [Authorize]
        public IActionResult Index()
        {
            var listing_campaign = _ICampaignBusiness.GetListCampaign(1, 10);
            if(listing_campaign != null)
            {
                ViewBag.Listing_Campaign = listing_campaign;
            }


            return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
