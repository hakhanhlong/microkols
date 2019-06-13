using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;

namespace Website.Controllers
{
    [Authorize(Roles = "Account")]
    public class AccountCampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly ISharedService _sharedService;
        private readonly IAccountService _accountService;
        public AccountCampaignController(ISharedService sharedService,
             IAccountService accountService,
            ICampaignService campaignService)
        {
            _campaignService = campaignService;
            _sharedService = sharedService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(string kw, int pageindex = 1, int pagesize = 20)
        {
            var model = await _campaignService.GetListCampaignByAccount(CurrentUser.Id, kw, pageindex, pagesize);

            return View(model);
        }


        #region Details

        public async Task<IActionResult> Details(int id)
        {
            var model = await _campaignService.GetCampaignDetailsByAccount(CurrentUser.Id, id);
            if (model == null) return NotFound();
            var campaignAccount = await _campaignService.GetCampaignAccountByAccount(CurrentUser.Id, model.Id);

            if (campaignAccount == null) return NotFound();
            ViewBag.CampaignAccount = campaignAccount;

            return View(model);
        }

        #endregion

        #region Action
        public async Task<IActionResult> ConfirmJoinCampaign(int campaignid)
        {
            var result = await _campaignService.ConfirmJoinCampaignByAccount(CurrentUser.Id, campaignid, CurrentUser.Username);

            this.AddAlert(result);

            return RedirectToAction("Details", new { id = campaignid });
        }



        #endregion


    }
}