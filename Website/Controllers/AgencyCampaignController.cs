using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Controllers
{
    [Authorize(Roles = "Agency")]
    public class AgencyCampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly ISharedService _sharedService;
        public AgencyCampaignController(ISharedService sharedService, ICampaignService campaignService)
        {
            _campaignService = campaignService;
            _sharedService = sharedService;
        }



        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            await ViewbagData();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _campaignService.CreateCampaign(CurrentUser.Id, model, CurrentUser.Username);
                if (id > 0)
                {
                    return RedirectToAction("Index");
                }

            }
            await ViewbagData();
            return View(model);
        }

        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypes = await _campaignService.GetCampaignTypes();
            ViewBag.Cities = await _sharedService.GetCities();
        }



    }
}