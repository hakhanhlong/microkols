﻿using System;
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
    [Authorize(Roles = "Account")]
    public class AccountController : BaseController
    {

        private readonly IAccountService _accountService;

        private readonly INotificationService _INotificationService;

        private readonly ISharedService _sharedService;
        private readonly IFileHelper _fileHelper;
        private readonly IFacebookHelper _facebookHelper;
        private readonly ICampaignService _campaignService;
        private readonly IFacebookJob _facebookJob;
        private readonly IBankService _IBankService;
        public AccountController(IAccountService accountService, ISharedService sharedService,
            ICampaignService campaignService,
            IFileHelper fileHelper, IFacebookHelper facebookHelper, IFacebookJob facebookJob, INotificationService __INotificationService, IBankService __IBankService)
        {
            _campaignService = campaignService;
            _accountService = accountService;
            _sharedService = sharedService;
            _fileHelper = fileHelper;
            _facebookHelper = facebookHelper;
            _facebookJob = facebookJob;
            _INotificationService = __INotificationService;
            _IBankService = __IBankService;

        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("ChangeInfo");
            //var account = await _accountService.GetAccount(CurrentUser.Id);

            //ViewBag.Counter = await _campaignService.GetCampaignCounterByAccount(CurrentUser.Id);

            //ViewBag.FbPosts = await _accountService.GetAccountFbPosts(CurrentUser.Id, 0, 1, 20);
            //ViewBag.ProfileUrl = await _accountService.GetFacebookProfileUrl(CurrentUser.Id);
            //ViewBag.Accounts = await _accountService.GetAccounts(AccountType.All, string.Empty, string.Empty, 1, 20);
            //return View(account);
        }

        public async Task<IActionResult> GetAccountUpdateInfoStatus()
        {
            var r = await _accountService.GetAccountUpdateInfoStatus(CurrentUser.Id);
            return Json(r);
        }

        #region Fb Post

        public async Task<IActionResult> FbAccount()
        {
            var accounts = await _accountService.GetAccounts(AccountType.All, string.Empty, string.Empty, 1, 200);
            ViewBag.Accounts = accounts.Accounts;
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
        public async Task<IActionResult> FbPost(int type = 0, int page = 1, int pagesize = 100)
        {
            var model = await _accountService.GetAccountFbPosts(CurrentUser.Id, type, page, pagesize);
            ViewBag.Type = type;
            return View(model);
        }
         

        #endregion

        #region Change Facebook Url

        public async Task<IActionResult> ChangeFacebookUrl()
        {
            var model = await _accountService.GetChangeFacebookUrl(CurrentUser.Id);

            if (!string.IsNullOrEmpty(model.FacebookUrl))
            {
                if (model.FacebookUrl.Contains("app_scoped_user_id"))
                {
                    model.FacebookUrl = "";
                }
            }

            

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeFacebookUrl(ChangeFacebookUrlViewModel model)
        {
            var r = await _accountService.ChangeFacebookUrl(CurrentUser.Id, model);

            if (r)
            {
                return RedirectToAction("ChangeInfo", new { vtype = 1 });
            }
            this.AddAlert(r);
            return RedirectToAction("ChangeFacebookUrl");
        }
        #endregion

        #region Change Avatar


        [HttpPost]
        public async Task<IActionResult> ChangeAvatar(IFormFile file)
        {
            var newpath = await _fileHelper.UploadTempFile(file);
            var avatar = _fileHelper.MoveTempFile(newpath, "avatar");

            var r = await _accountService.ChangeAvatar(CurrentUser.Id, avatar, CurrentUser.Username);
            this.AddAlert(r);
            if (r)
            {
                await ReSignIn(CurrentUser.Id);
            }
            return RedirectToAction("ChangeInfo");
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
                return RedirectToAction("ChangePassword");

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

                //########### Longhk add create notification ##########################################################
                string _msg = string.Format("Influencer {0}, gửi duyệt thông tin xác minh danh tính", CurrentUser.Username);
                string _data = "Influencer";
                await _INotificationService.CreateNotification(CurrentUser.Id, EntityType.System, 0, NotificationType.AccountSendVerify, _msg, _data);

                //#####################################################################################################

                this.AddAlert(r);
                return RedirectToAction("ChangeIDCard");

            }
            return View(model);
        }
        #endregion

        #region Change Info
        public async Task<IActionResult> ChangeInfo(int vtype = 0)
        {
            var model = await _accountService.GetInformation(CurrentUser.Id);            
            var accountProvider = await _accountService.GetAccountProviderByAccount(CurrentUser.Id, AccountProviderNames.Facebook);
            ViewBag.Categories = await _sharedService.GetCategories();
            model.FacebookProfile = accountProvider.FbProfileLink;

            try {
                DateTime birthday = DateTime.ParseExact(model.Birthday, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (birthday != null)
                {
                    if ((DateTime.Now.Year - birthday.Year) < 18)
                    {
                        //TempData["MessageWarning"] = "Bạn chưa đủ 18 tuổi!";
                    }
                }
                model.Birthday = birthday.ToString("dd/MM/yyyy");
            }
            catch { }
            
            if (vtype == 1)
            {
                return View("AuthChangeInfo", model);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeInfo(ChangeInformationViewModel model, int vtype = 0)
        {
            if (ModelState.IsValid)
            {                                
                var r = await _accountService.ChangeInformation(CurrentUser.Id, model, CurrentUser.Username);
                await _accountService.UpdateFacebookUrlProfile(CurrentUser.Id, AccountProviderNames.Facebook, model.FacebookProfile);
                if (vtype == 1 && r)
                {
                    return RedirectToAction("ChangeContact", new { vtype = 1 });
                }
                this.AddAlert(r);
                return RedirectToAction("ChangeInfo");

            }
            ViewBag.Categories = await _sharedService.GetCategories();
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


        public async Task<IActionResult> ChangeContact(int vtype = 0)
        {
            var model = await _accountService.GetContact(CurrentUser.Id);
            await ViewbagAddress();
            if (vtype == 1)
            {
                return View("AuthChangeContact", model);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeContact(ChangeContactViewModel model, int vtype = 0)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeContact(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r);
                if (r)
                {
                    await ReSignIn(CurrentUser.Id);
                    if (vtype == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction("ChangeContact");
                }

            }
            await ViewbagAddress();
            return View(model);
        }

        #endregion


        #region ChangeBankAccount
        public async Task<IActionResult> ChangeBankAccount()
        {
            var model = await _accountService.GetBankAccount(CurrentUser.Id);

            //############# anh Long bổ sung ##############################################################################
            var banks = await _IBankService.ListAll();
            var list_bank = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            list_bank.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = "Chọn ngân hàng", Value = "" });
            foreach (var bank in banks)
            {
                string bankName = string.Format("{0}({1})", bank.VietName, bank.TradingName);
                list_bank.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = bankName, Value = bankName });
            }
            ViewBag.ListingBank = list_bank;
            //#############################################################################################################


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeBankAccount(ChangeBankAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _accountService.ChangeBankAccount(CurrentUser.Id, model, CurrentUser.Username);

                this.AddAlert(r, "Bạn cập nhật thông tin ngần hàng thành công!");

                

                //return RedirectToAction("ChangeBankAccount");

            }
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


            ViewBag.AccountCampaignCharges = await _accountService.GetAccountCampaignCharges(CurrentUser.Id);

            //if (model.Type != AccountType.Regular)
            //{
            //    ViewBag.AccountCampaignCharges = await _accountService.GetAccountCampaignCharges(CurrentUser.Id);
            //}
            //else
            //{
            //    ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            //}

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
                        Min = model.Min[i],
                        Max = model.Max[i],
                        //Kpi = model.Kpi[i],
                        Kpi = 0,
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