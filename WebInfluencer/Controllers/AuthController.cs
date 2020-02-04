using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.ViewModels;

namespace WebInfluencer.Controllers
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
        public IActionResult Login()
        {
            return View();
        }


        #endregion


        #region Social login


        public IActionResult SigninFacebook()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetUserInfo(AccountProviderNames provider, string token)
        {
            var loginInfo = provider == AccountProviderNames.Facebook ? await _facebookHelper.GetLoginProviderAsync(token) :
                 await SocialHelper.VerifyGoogleTokenAsync(token);
            if (loginInfo == null)
            {
                this.AddAlertDanger($"Lỗi khi lấy thông tin từ hệ thống {provider}. Xin vui lòng thử lại. Token {token}");
                return RedirectToAction("Login");
            }

            var accountProvider = await _accountService.GetAccountProviderByProvider(provider, loginInfo.ProviderId, token);
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


        public IActionResult LoginFb(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }
            return new ChallengeResult(
                FacebookDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl })
                });
        }

        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return BadRequest(); // TODO: Handle this better.

            var token = string.Empty;
            var val = authenticateResult.Ticket.Properties.Items.TryGetValue(".Token.access_token", out token);

            var loginInfo = await _facebookHelper.GetLoginProviderAsync(token);
            if (loginInfo == null)
            {
                this.AddAlertDanger($"Lỗi khi lấy thông tin từ hệ thống. Xin vui lòng thử lại. Token {token}");
                return RedirectToAction("Login");
            }

            var accountProvider = await _accountService.GetAccountProviderByProvider(AccountProviderNames.Facebook, loginInfo.ProviderId, token);
            var accountProviderExist = accountProvider != null;

            var auth = await _accountService.GetAuth(loginInfo);
            if (auth == null)
            {
                this.AddAlertDanger("Lỗi khi lấy thông tin đăng nhập. Tài khoản đã bị khóa hoặc xóa. Xin vui lòng liên hệ quản trị hệ thống");
                return RedirectToAction("Login");
            }

            /*
             * Ko update token - lay pots fb nua 

            BackgroundJob.Enqueue<IFacebookJob>(m => m.ExtendAccessToken());
            if (!accountProviderExist)
            {
                BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(auth.Id, auth.Username, 1));
            }

            */


            await SignIn(auth);

            return RedirectToAction("Index", "Account");

            //return LocalRedirect("/");
        }

        #endregion


        #endregion


        public async Task<IActionResult> VerifyEmail(string email)
        {
            var r = await _accountService.VerifyEmail(email);

            return Json(r);

        }

        #endregion




        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await SignOut();
            return RedirectToAction("Login");
        }
    }
}