using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class CampaignTypeChargeController : Controller
    {

        private readonly ICampaignTypeChargeRepository _ICampaignTypeChargeRepository;

        public CampaignTypeChargeController(ICampaignTypeChargeRepository __ICampaignTypeChargeRepository)
        {
            _ICampaignTypeChargeRepository = __ICampaignTypeChargeRepository;
        }

        public IActionResult Index()
        {
            var campaign_type_listing = _ICampaignTypeChargeRepository.ListAll();
            return View(campaign_type_listing);
        }

        public IActionResult Edit(int id = 0)
        {

            var _CampaignTypeCharge = _ICampaignTypeChargeRepository.GetById(id);
            CampaignTypeChargeViewModel _CampaignTypeChargeViewModel = null;
            if (_CampaignTypeCharge != null)
            {
                _CampaignTypeChargeViewModel = new CampaignTypeChargeViewModel(_CampaignTypeCharge);
            }
            else
            {
                TempData["MessageError"] = "Campaign Type Do Not Existing";
            }

            return View(_CampaignTypeChargeViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(CampaignTypeChargeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _CampaignTypeCharge = _ICampaignTypeChargeRepository.GetById(model.Id);
                if (_CampaignTypeCharge != null)
                {
                    _CampaignTypeCharge.AccountChargeAmount = model.AccountChargeAmount;
                    _CampaignTypeCharge.ServiceChargeAmount = model.ServiceChargeAmount;
                    _CampaignTypeCharge.AccountChargeExtraPercent = model.AccountChargeExtraPercent;
                    try
                    {
                        await _ICampaignTypeChargeRepository.UpdateAsync(_CampaignTypeCharge);
                        TempData["MessageSuccess"] = string.Format("Update Campaign Type {0} successfully", model.Id);
                    }
                    catch (Exception ex)
                    {
                        TempData["MessageError"] = string.Format("Error: {0}", ex.Message);
                    }
                }
                else
                {
                    TempData["MessageError"] = "Campaign Type Do Not Existing";
                }
            }
            else
            {
                TempData["MessageError"] = string.Format("{0}", "ModelState Invalid");
            }

            return RedirectToAction("Edit", "CampaignTypeCharge", new { id = model.Id });

        }

    }
}