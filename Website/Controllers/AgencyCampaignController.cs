using System;
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
        private readonly INotificationService _notificationService;
        public AgencyCampaignController(ISharedService sharedService, ICampaignService campaignService, INotificationService notificationService)
        {
            _campaignService = campaignService;
            _sharedService = sharedService;
            _notificationService = notificationService;
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
                if(model.AccountType == null || model.AccountType.Count== 0)
                {
                    ModelState.AddModelError("AccountType", "Hãy chọn đối tượng");
                }
                else
                {
                    var id = await _campaignService.CreateCampaign(CurrentUser.Id, model, CurrentUser.Username);
                    if (id > 0)
                    {
                        this.AddAlertSuccess("Thêm chiến dịch mới thành công");
                        return RedirectToAction("Index");
                    }
                }

            }
            await ViewbagData();
            return View(model);
        }
        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            ViewBag.Cities = await _sharedService.GetCities();
        }


        #endregion


        #region RequestAccount

        public async Task<IActionResult> RequestAccountJoinCampaign(int campaignid, int accountid)
        {
            return Json(1);
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