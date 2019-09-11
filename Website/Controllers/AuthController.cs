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
    public class AuthController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IAgencyService _agencyService;
        private readonly ISharedService _sharedService;
        private readonly IFacebookHelper _facebookHelper;
        public AuthController(IAccountService accountService, ISharedService sharedService, IAgencyService agencyService, IFacebookHelper facebookHelper)
        {

            _accountService = accountService;
            _sharedService = sharedService;
            _agencyService = agencyService;
            _facebookHelper = facebookHelper;

        }


        #region Account

        #region Login
        //[Route("~/dang-nhap")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[Route("~/dang-nhap")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = "")
        {
            if (ModelState.IsValid)
            {
                var auth = await _accountService.GetAuth(model);

                if (auth != null)
                {
                    await SignIn(auth);
                    if (!string.IsNullOrEmpty(returnurl)) return Redirect(returnurl);

                    return RedirectToAction("Index", "Home");
                }
                this.AddAlertDanger("Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(model);
        }



        #endregion


        #region Social login
        [HttpPost]
        public async Task<IActionResult> GetUserInfo(AccountProviderNames provider, string token)
        {
            var loginInfo = provider == AccountProviderNames.Facebook ? await _facebookHelper.GetLoginProviderAsync(token) :
                 await Code.Helpers.SocialHelper.VerifyGoogleTokenAsync(token);
            if (loginInfo == null)
            {
                this.AddAlertDanger($"Lỗi khi lấy thông tin từ hệ thống {provider}. Xin vui lòng thử lại. Token {token}");
                return RedirectToAction("Login");
            }

            var accountProvider = await _accountService.GetAccountProviderByProvider(provider, loginInfo.ProviderId);
            var accountProviderExist = accountProvider != null;

            var auth = await _accountService.GetAuth(loginInfo);
            if (auth == null)
            {
                this.AddAlertDanger("Lỗi khi lấy thông tin đăng nhập. Tài khoản đã bị khóa hoặc xóa. Xin vui lòng liên hệ quản trị hệ thống");
                return RedirectToAction("Login");
            }

            if (provider == AccountProviderNames.Facebook)
            {
                BackgroundJob.Enqueue<IFacebookJob>(m => m.ExtendAccessToken());
                if (!accountProviderExist)
                {
                    BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(auth.Id, auth.Username, 1));
                }
            }


            await SignIn(auth);
            return RedirectToAction("Index", "Account");
        }


        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl = "")
        {
            if (ModelState.IsValid)
            {
                var id = await _accountService.Register(model);

                if (id > 0)
                {
                    var auth = await _accountService.GetAuth(id);

                    if (auth != null)
                    {
                        await SignIn(auth);
                        if (!string.IsNullOrEmpty(returnurl)) return Redirect(returnurl);

                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                this.AddAlertDanger("Tên đăng nhập đã tồn tại");
            }
            return View(model);
        }



        #endregion


        public async Task<IActionResult> VerifyEmail(string email)
        {
            var r = await _accountService.VerifyEmail(email);

            return Json(r);

        }

        #endregion




        #region Agency

        #region Login
        public IActionResult AgencyLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgencyLogin(AgencyLoginViewModel model, string returnurl = "")
        {
            if (ModelState.IsValid)
            {
                var auth = await _agencyService.GetAuth(model);

                if (auth != null)
                {
                    await SignIn(auth);
                    if (!string.IsNullOrEmpty(returnurl)) return Redirect(returnurl);

                    return RedirectToAction("Index", "Home");
                }
                this.AddAlertDanger("Tên đăng nhập hoặc mật khẩu không đúng");
            }
            return View(model);
        }



        #endregion

        #region Register
        public IActionResult AgencyRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgencyRegister(RegisterAgencyViewModel model, string returnurl = "")
        {
            if (ModelState.IsValid)
            {
                var id = await _agencyService.Register(model);

                if (id > 0)
                {
                    this.AddAlertSuccess("Đăng ký doanh nghiệp thành công. Vui lòng chờ ban quản trị duyệt");
                    return RedirectToAction("AgencyRegister");
                }
                this.AddAlertDanger("Tên đăng nhập đã tồn tại");
            }
            return View(model);
        }



        #endregion

        public async Task<IActionResult> VerifyAgencyUsername(string username)
        {
            if(username.Contains("google") || username.Contains("yahoo")|| username.Contains("gmail"))
            {
                return Json(false);
            }
            var r = await _agencyService.VerifyUsername(username);

            return Json(r);

        }


        #endregion


        [Authorize]
        //[Route("~/thoat")]
        public async Task<IActionResult> Logout()
        {
            var type = CurrentUser.Type;
            await SignOut();

            if (type == Core.Entities.EntityType.Agency)
            {
                return RedirectToAction("AgencyLogin");
            }
            return RedirectToAction("Index","Home");
        }
    }
}