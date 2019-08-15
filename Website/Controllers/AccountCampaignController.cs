using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Code.Helpers;
using Website.Interfaces;
using Website.Jobs;
using Website.ViewModels;

namespace Website.Controllers
{
    [Authorize(Roles = "Account")]
    public class AccountCampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly ISharedService _sharedService;
        private readonly IAccountService _accountService;
        private readonly IFacebookHelper _facebookHelper;
        public AccountCampaignController(ISharedService sharedService,
            IFacebookHelper facebookHelper,
             IAccountService accountService,
            ICampaignService campaignService)
        {
            _facebookHelper = facebookHelper;
            _campaignService = campaignService;
            _sharedService = sharedService;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(string kw,int type = 1, int pageindex = 1, int pagesize = 20)
        {
            ViewBag.Type = type;
            ViewBag.Kw = kw;
            var model = await _campaignService.GetListCampaignByAccount(CurrentUser.Id, type, kw, pageindex, pagesize);

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
            ViewBag.FacebookId = await _accountService.GetProviderIdByAccount(CurrentUser.Id, AccountProviderNames.Facebook);

            return View(model);
        }



        #endregion

        #region Action
        public async Task<IActionResult> FeedbackJoinCampaign(int campaignid, int type)
        {
            var result = await _campaignService.FeedbackJoinCampaignByAccount(CurrentUser.Id, campaignid, CurrentUser.Username, type == 1);


            //this.AddAlert(result);

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


        [HttpPost]
        public async Task<IActionResult> SubmitCampaignAccountSharedContent()
        {
            BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(CurrentUser.Id, CurrentUser.Username, 2));
            ViewBag.Success = "Hệ thống đang cập nhật thông tin Facebook của bạn. Vui lòng đợi";

            return PartialView("UpdateCampaignAccountMessage");
        }


        public async Task<IActionResult> SubmmitCampaignAccountChangeAvatar(int campaignid)
        {

            var facebookid = await _accountService.GetProviderIdByAccount(CurrentUser.Id, AccountProviderNames.Facebook);

            ViewBag.AvatarUrl = _facebookHelper.GetAvatarUrl(facebookid);
            return PartialView(new SubmmitCampaignAccountChangeAvatarViewModel()
            {
                CampaignId = campaignid
            });
        }
        [HttpPost]
        public async Task<IActionResult> SubmmitCampaignAccountChangeAvatar(SubmmitCampaignAccountChangeAvatarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.SubmmitCampaignAccountChangeAvatar(CurrentUser.Id, model, CurrentUser.Username);
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




        public async Task<IActionResult> UpdateCampaignAccountRef(int campaignid, CampaignType campaignType)
        {

            if (campaignType == CampaignType.ChangeAvatar)
            {
                return RedirectToAction("SubmmitCampaignAccountChangeAvatar", new { campaignid });
            }

            var campaignAccount = await _campaignService.GetCampaignAccountByAccount(CurrentUser.Id, campaignid);
            if (campaignAccount == null)
            {
                return PartialView();
            }
            if ((campaignAccount.Status == CampaignAccountStatus.Confirmed || campaignAccount.Status == CampaignAccountStatus.DeclinedContent)
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