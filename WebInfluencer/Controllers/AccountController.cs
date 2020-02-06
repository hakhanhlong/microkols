using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.ViewModels;

namespace WebInfluencer.Controllers
{
    [Authorize(Roles = "Account")]
    public class AccountController : BaseController
    {

        private readonly IAccountService _accountService;
        private readonly ISharedService _sharedService;
        private readonly IFileHelper _fileHelper;
        private readonly IFacebookHelper _facebookHelper;
        private readonly ICampaignService _campaignService;
        private readonly IFacebookJob _facebookJob;
        public AccountController(IAccountService accountService, ISharedService sharedService,
            ICampaignService campaignService,
            IFileHelper fileHelper, IFacebookHelper facebookHelper, IFacebookJob facebookJob)
        {
            _campaignService = campaignService;
            _accountService = accountService;
            _sharedService = sharedService;
            _fileHelper = fileHelper;
            _facebookHelper = facebookHelper;
            _facebookJob = facebookJob;


        }

        public async Task<IActionResult> Index()
        {
            var account = await _accountService.GetAccount(CurrentUser.Id);

            ViewBag.Counter = await _campaignService.GetCampaignCounterByAccount(CurrentUser.Id);

            ViewBag.FbPosts = await _accountService.GetAccountFbPosts(CurrentUser.Id, 0, 1, 20);
            ViewBag.ProfileUrl = await _accountService.GetFacebookProfileUrl(CurrentUser.Id);
            ViewBag.Accounts = await _accountService.GetAccounts(AccountType.All, string.Empty, string.Empty, 1, 20);
            return View(account);
        }


        #region Fb Post

        public async Task<IActionResult> FbAccount()
        {
            ViewBag.Accounts = await _accountService.GetAccounts(AccountType.All, string.Empty, string.Empty, 1, 200);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateFbPost(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                var r = await _accountService.UpdateAccountProvidersAccessToken(CurrentUser.Id, accessToken, 0);


                await _facebookJob.UpdateFbPost(CurrentUser.Id, CurrentUser.Username, 2);
                this.AddAlertSuccess("Bạn đã đặt lệnh cập nhật thông tin Facebook thành công. Vui lòng chờ 1 - 2 phút để hệ thống tự động cập nhật thông tin bài chia sẻ của bạn");

                BackgroundJob.Enqueue<IFacebookJob>(m => m.ExtendAccessToken());
            }
            else
            {
                this.AddAlertDanger("Hãy chấp nhận quyền trên Facebook");
            }


            return RedirectToAction("FbPost", new { type = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFbFriends(string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {

                return RedirectToAction("FbAccount");
                //var r = await _accountService.UpdateAccountProvidersAccessToken(CurrentUser.Id, accessToken, 0);


                //await _facebookJob.UpdateFbPost(CurrentUser.Id, CurrentUser.Username, 2);
                //this.AddAlertSuccess("Bạn đã đặt lệnh cập nhật thông tin Facebook thành công. Vui lòng chờ 1 - 2 phút để hệ thống tự động cập nhật thông tin bài chia sẻ của bạn");

                //BackgroundJob.Enqueue<IFacebookJob>(m => m.ExtendAccessToken());
            }
            else
            {
                this.AddAlertDanger("Hãy chấp nhận quyền trên Facebook");
            }


            return RedirectToAction("Index");


        }
        public async Task<IActionResult> FbPost(int type = 0, int page = 1, int pagesize = 20)
        {
            var model = await _accountService.GetAccountFbPosts(CurrentUser.Id, type, page, pagesize);
            ViewBag.Type = type;
            return View(model);
        }


        #endregion

        #region Change Facebook Url
        [Authorize]
        public async Task<IActionResult> ChangeFacebookUrl()
        {
            var model = await _accountService.GetChangeFacebookUrl(CurrentUser.Id);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeFacebookUrl(ChangeFacebookUrlViewModel model)
        {
            var r = await _accountService.ChangeFacebookUrl(CurrentUser.Id, model);

            if (r)
            {
                return RedirectToAction("ChangeInfo");
            }
            this.AddAlert(r);
            return RedirectToAction("ChangeFacebookUrl");
        }
        #endregion

        #region ChangePassword
        public async Task<IActionResult> ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangePassword(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeIDCard");

            }
            return View(model);
        }
        #endregion

        #region ChangeIDCard
        public async Task<IActionResult> ChangeIDCard()
        {
            var model = await _accountService.GetIDCard(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeIDCard(ChangeIDCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                //--> move temp folder -> resources

                model.ImageBack = _fileHelper.MoveTempFile(model.ImageBack, "account");
                model.ImageFront = _fileHelper.MoveTempFile(model.ImageFront, "account");

                var r = await _accountService.ChangeIDCard(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeIDCard");

            }
            return View(model);
        }
        #endregion

        #region Change Info
        public async Task<IActionResult> ChangeInfo()
        {
            var model = await _accountService.GetInformation(CurrentUser.Id);
            ViewBag.Categories = await _sharedService.GetCategories();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeInfo(ChangeInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeInformation(CurrentUser.Id, model, CurrentUser.Username);
                if (r)
                {
                    return RedirectToAction("ChangeContact");
                }
                this.AddAlert(r);
                return RedirectToAction("ChangeInfo");

            }
            ViewBag.Categories = await _sharedService.GetCategories();
            return View(model);
        }
        #endregion

        #region ChangeBankAccount
        public async Task<IActionResult> ChangeBankAccount()
        {
            var model = await _accountService.GetBankAccount(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeBankAccount(ChangeBankAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeBankAccount(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                return RedirectToAction("ChangeBankAccount");

            }
            return View(model);
        }
        #endregion


        #region Change Contact
        private async Task ViewbagAddress()
        {
            var cities = await _sharedService.GetCities();
            ViewBag.Cities = cities;
            var city = cities.FirstOrDefault();
            var districtid = city.Id;
            ViewBag.Districts = await _sharedService.GetDistricts(districtid);
        }


        public async Task<IActionResult> ChangeContact()
        {
            var model = await _accountService.GetContact(CurrentUser.Id);
            await ViewbagAddress();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeContact(ChangeContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeContact(CurrentUser.Id, model, CurrentUser.Username);

                if (r)
                {
                    await ReSignIn(CurrentUser.Id);
                    return RedirectToAction("Index","Home");
                }

                this.AddAlert(r);
            }
            await ViewbagAddress();
            return View(model);
        }

        #endregion

        #region ChangeAccountType

        public async Task<IActionResult> UpdateIgnoreCampaignTypes(CampaignType type, bool removed)
        {
            var result = await _accountService.UpdateIgnoreCampaignTypes(CurrentUser.Id, type, !removed, CurrentUser.Username);
            return Json(result);
        }

        public async Task<IActionResult> ChangeAccountType()
        {
            var model = await _accountService.GetChangeAccountType(CurrentUser.Id);
            if (model.Type != AccountType.Regular)
            {
                ViewBag.AccountCampaignCharges = await _accountService.GetAccountCampaignCharges(CurrentUser.Id);
            }
            else
            {
                ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            }
            ViewBag.IgnoreCampaignTypes = await _accountService.GetIgnoreCampaignTypes(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeAccountType(ChangeAccountTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeAccountType(CurrentUser.Id, model, CurrentUser.Username);

                if (r)
                {
                    await ReSignIn(CurrentUser.Id);
                    this.AddAlert(true);
                    return RedirectToAction("ChangeAccountType");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateAccountCampaignCharge(UpdateAccountCampaignChargeViewModel model)
        {
            if (ModelState.IsValid)
            {
                for (var i = 0; i < model.Id.Count; i++)
                {

                    await _accountService.UpdateAccountCampaignCharge(CurrentUser.Id, new AccountCampaignChargeViewModel()
                    {
                        Id = model.Id[i],
                        AccountChargeAmount = model.AccountChargeAmount[i],
                        Type = model.Type[i]
                    });
                }
                this.AddAlert(true);
            }
            return RedirectToAction("ChangeAccountType");
        }

        #endregion

        #region Link Provider
        [HttpPost]
        public async Task<IActionResult> LinkProvider(AccountProviderNames provider, string token, string returnurl)
        {
            var info = provider == AccountProviderNames.Facebook ? await _facebookHelper.GetLoginProviderAsync(token) :
               await SocialHelper.VerifyGoogleTokenAsync(token);
            if (info == null)
            {
                this.AddAlertDanger($"Lỗi khi lấy thông tin từ hệ thống {provider}. Xin vui lòng thử lại. Token {token}");
            }
            else
            {
                var r = await _accountService.UpdateAccountProvider(CurrentUser.Id, new UpdateAccountProviderViewModel()
                {
                    Email = info.Email,
                    Image = info.Image,
                    Name = info.Name,
                    Provider = provider,
                    ProviderId = info.ProviderId
                }, CurrentUser.Username);

                if (r < 0)
                {
                    this.AddAlertDanger($"Tài khoản {info.Provider} đã được liên kết với tài khoản khác. Vui lòng thử lại tài khoản khác");
                }
                else
                {

                    BackgroundJob.Enqueue<IFacebookJob>(m => m.ExtendAccessToken());
                    this.AddAlertSuccess($"Liên kết Tài khoản {info.Provider} thành công");


                    if (r == 2)
                    { // tao moi fb id -> add new FbPost tu 2018
                        BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(CurrentUser.Id, CurrentUser.Username, 1));
                    }
                }
            }

            if (!string.IsNullOrEmpty(returnurl)) { return Redirect(returnurl); }
            return RedirectToAction("Index");
        }
        #endregion




        #region Helper
        private async Task ReSignIn(int id)
        {
            var auth = await _accountService.GetAuth(CurrentUser.Id);
            await SignIn(auth);
        }
        #endregion


    }
}