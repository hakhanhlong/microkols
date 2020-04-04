using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.ViewModels;

namespace WebInfluencer.Controllers
{

    public class CampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly ICampaignAccountCaptionService _campaignAccountCaptionService;
        private readonly ICampaignAccountContentService _campaignAccountContentService;
        private readonly ICampaignAccountStatisticService _campaignAccountStatisticService;


        private readonly ISharedService _sharedService;
        private readonly IAccountService _accountService;
        private readonly IFacebookHelper _facebookHelper;
        private readonly IFileHelper _fileHelper;
        public CampaignController(ISharedService sharedService,
            IFacebookHelper facebookHelper, IFileHelper fileHelper,
            ICampaignAccountCaptionService campaignAccountCaptionService,
            ICampaignAccountContentService campaignAccountContentService,
            ICampaignAccountStatisticService campaignAccountStatisticService,
             IAccountService accountService,
            ICampaignService campaignService)
        {
            _campaignAccountCaptionService = campaignAccountCaptionService;
            _campaignAccountContentService = campaignAccountContentService;
            _campaignAccountStatisticService = campaignAccountStatisticService;
            _facebookHelper = facebookHelper;
            _campaignService = campaignService;
            _fileHelper = fileHelper;
            _sharedService = sharedService;
            _accountService = accountService;
        }



        public async Task<IActionResult> MarketPlace(string kw, CampaignType? type, int pageindex = 1, int pagesize = 20)
        {
            ViewBag.Kw = kw;
            ViewBag.type = type;
            var model = await _campaignService.GetCampaignMarketPlaceByAccount(CurrentUser.Id, type, kw, pageindex, pagesize);

            return View(model);
        }
        public async Task<IActionResult> Details(int id, int tab = 0, int pageindex = 1, int pagesize = 20)
        {
            var model = await _campaignService.GetCampaignMarketPlace(id);
            ViewBag.Tab = tab;
            
            if (tab == 1)
            {
                var captionaccount = model.CampaignAccounts.FirstOrDefault(m => m.AccountId == CurrentUser.Id);
                if (captionaccount != null)
                {
                    ViewBag.Statistics = await _campaignAccountStatisticService.GetCampaignAccountStatistics(captionaccount.Id, string.Empty, pageindex, pagesize);
                }
                return View("DetailsStatistic", model);
            }
            if (tab == 2)
            {
                var captionaccount = model.CampaignAccounts.FirstOrDefault(m => m.AccountId == CurrentUser.Id);
                if (captionaccount != null)
                {
                    var captions = await _campaignAccountCaptionService.GetCampaignAccountCaptions(captionaccount.Id, string.Empty, pageindex, pagesize);
                    ViewBag.Captions = captions;
                }

                return View("DetailsCaption", model);
            }
            if (tab == 3)
            {
                var captionaccount = model.CampaignAccounts.FirstOrDefault(m => m.AccountId == CurrentUser.Id);
                if (captionaccount != null)
                {
                    ViewBag.Contents = await _campaignAccountContentService.GetCampaignAccountContents(captionaccount.Id, string.Empty, pageindex, pagesize);
                }

                return View("DetailsContent", model);
            }
            ViewBag.AccountStatus = await _accountService.GetAccountStatus(CurrentUser.Id);
            return View(model);
        }
        #region Caption
        public async Task<IActionResult> CreateCaption(CreateCampaignAccountCaptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignAccountCaptionService.CreateCampaignAccountCaption(model, CurrentUser.Username);
                SetMessageModal("Đã gửi caption thành công, caption của bạn đang chờ được xét duyệt.");
            }
            return RedirectToAction("Details", new { id = model.CampaignId, tab = 2 });
        }
        #endregion

        #region Content
        public async Task<IActionResult> CreateContent(CreateCampaignAccountContentViewModel model, List<IFormFile> file)
        {
            if (ModelState.IsValid)
            {
                var imgs = new List<string>();
                if (file != null)
                    foreach (var item in file)
                    {

                        var newpath = await _fileHelper.UploadTempFile(item);
                        var tmp = _fileHelper.MoveTempFile(newpath, "campaigncontent");
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            imgs.Add(tmp);
                        }
                    }

                model.Image = imgs;

                var r = await _campaignAccountContentService.CreateCampaignAccountContent(model, CurrentUser.Username);
                SetMessageModal("Đã gửi nội dung thành công, Nội dung của bạn đang chờ được xét duyệt.");
            }
            return RedirectToAction("Details", new { id = model.CampaignId, tab = 3 });
        }
        #endregion

        #region Action
        public async Task<IActionResult> FeedbackJoinCampaign(RequestJoinCampaignViewModel model, int type)
        {
            var result = await _campaignService.FeedbackJoinCampaignByAccount(CurrentUser.Id, model, CurrentUser.Username, type == 1);


            this.AddAlert(true, type == 1 ? "Bạn đã đồng ý tham gia chiến dịch" : "Bạn đã từ chối tham gia chiến dịch");

            return RedirectToAction("Details", new { id = model.CampaignId });
        }

        public async Task<IActionResult> RequestJoinCampaign(RequestJoinCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _campaignService.RequestJoinCampaignByAccount(CurrentUser.Id, model, CurrentUser.Username);
                if (result)
                {
                    SetMessageModal("Bạn đã yêu cầu tham gia chiến dịch thành công. Vui lòng chờ doanh nghiệp xét duyệt");
                }
                else
                {
                    this.AddAlertDanger("Bạn đã tham gia chiến dịch");
                }
            }
            else
            {

                this.AddAlertDanger("Thông tin không đúng vui lòng thử lại");
            }
            return RedirectToAction("Details", new { id = model.CampaignId });
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


       // [HttpPost]
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

        public async Task<IActionResult> UpdateCampaignAccountRefImages(int campaignid)
        {
            return PartialView(new UpdateCampaignAccountRefImagesViewModel()
            {
                CampaignId = campaignid
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCampaignAccountRefImages(UpdateCampaignAccountRefImagesViewModel model)
        {
            var r = await _campaignService.UpdateCampaignAccountRefImages(CurrentUser.Id, model, CurrentUser.Username);
            if (r > 0)
            {
                ViewBag.Success = "Cập nhật thành công hình ảnh";
            }
            else
            {
                ViewBag.Error = "Thông tin chiến dịch không đúng";
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

        public async Task<IActionResult> UpdateReviewAddress(int id, string reviewaddress)
        {
            var r = await _campaignService.UpdateReviewAddress(id, reviewaddress, CurrentUser.Username);
            if (r > 0)
            {
                ViewBag.Success = "Cập nhật địa chỉ nhận hàng thành công";
            }
            else
            {
                ViewBag.Error = "Thông tin chiến dịch không đúng";
            }


            return RedirectToAction("Details", new { id = r });
        }


        #endregion


    }
}