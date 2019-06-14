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
            ViewBag.FacebookId = await _accountService.GetProviderIdByAccount(CurrentUser.Id, "Facebook");

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

        public async Task<IActionResult> SubmitCampaignAccountRefContent(int campaignid)
        {
            return PartialView(new SubmitCampaignAccountRefContentViewModel()
            {
                CampaignId = campaignid
            });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCampaignAccountRefContent(SubmitCampaignAccountRefContentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.SubmitCampaignAccountRefContent(CurrentUser.Id, model, CurrentUser.Username);
                if (r > 0)
                {
                    ViewBag.Success = "Bạn đã gửi thành công nội dung Caption. Vui lòng chờ doanh nghiệp duyệt nội dung";
                }
                else
                {
                    ViewBag.Error = "Thông tin chiến dịch không đúng";
                }

            }
            else
            {
                ViewBag.Error = "Hãy nhập đầy đủ thông tin";
            }

            return PartialView("UpdateCampaignAccountMessage");
        }



        public async Task<IActionResult> UpdateCampaignAccountRef(int campaignid, CampaignType campaignType)
        {
            var campaignAccount = await _campaignService.GetCampaignAccountByAccount(CurrentUser.Id, campaignid);
            if (campaignAccount == null)
            {
                return PartialView();
            }
            if ((campaignAccount.Status == CampaignAccountStatus.Confirmed || campaignAccount.Status == CampaignAccountStatus.Declined)
                && campaignType == CampaignType.ShareContentWithCaption)
            {
                return RedirectToAction("SubmitCampaignAccountRefContent", new { campaignid });
            }


            return PartialView(new UpdateCampaignAccountRefViewModel()
            {
                CampaignId = campaignid,
                CampaignType = campaignType
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCampaignAccountRef(UpdateCampaignAccountRefViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.UpdateCampaignAccountRef(CurrentUser.Id, model, CurrentUser.Username);
                if (r > 0)
                {
                    ViewBag.Success = "Bạn đã thực hiện thành công công việc";
                }
                else
                {
                    ViewBag.Error = "Thông tin chiến dịch không đúng";
                }

            }
            else
            {
                ViewBag.Error = "Hãy nhập đầy đủ thông tin";
            }

            return PartialView("UpdateCampaignAccountMessage");
        }
        #endregion


    }
}