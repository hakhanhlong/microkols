﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
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



        public async Task<IActionResult> Index(CampaignType? type, string kw, int page = 1,int pagesize = 20)
        {
            var model = await _campaignService.GetListCampaignByAgency(CurrentUser.Id, type, kw, page, pagesize);
            ViewBag.Kw = kw;
            ViewBag.type = type;
            return View(model);
        }

        #region Create

        public async Task<IActionResult> Create()
        {
            await ViewbagData();
            return View(new CreateCampaignViewModel());
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
            ViewBag.CampaignTypePrices = await _campaignService.GetCampaignTypePrices();
            ViewBag.Cities = await _sharedService.GetCities();
        }


        #endregion

        public async Task<IActionResult> Details(int id)
        {
            var model = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, id);
            if (model == null) return NotFound();
            await ViewbagData();
            return View(model);
        }


    }
}