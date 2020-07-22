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
using Newtonsoft.Json;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.ViewModels;

namespace WebInfluencer.Controllers
{
    public class AuthController : BaseAuthController
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



        #region Login
        public IActionResult Login()
        {
            return View();
        }


        public IActionResult SigninFacebook()
        {
            return RedirectToAction("Login", "Auth");
        }

        #endregion


        #region Social login

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
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
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



            // check permission first #########################################################################
            var permissions = await _facebookHelper.GetPermissions(token);
            string _msg_permission = string.Empty;
            Permissions list_permissions;
            if (permissions != null)
            {
                string data = JsonConvert.SerializeObject(permissions);
                list_permissions = JsonConvert.DeserializeObject<Permissions>(data);
                bool check_permission = true;
                foreach(var peritem in list_permissions.data)
                {

                    if(peritem.permission == "email" && peritem.status == "declined")
                    {
                        _msg_permission += "Địa chỉ email,";
                        check_permission = false;
                    }
                    if (peritem.permission == "user_link" && peritem.status == "declined")
                    {
                        _msg_permission += "Liên kết dòng thời gian,";
                        check_permission = false;
                    }
                    if (peritem.permission == "user_posts" && peritem.status == "declined")
                    {
                        _msg_permission += "Bài viết trên dòng thời gian,";
                        check_permission = false;
                    }
                    if (peritem.permission == "user_friends" && peritem.status == "declined")
                    {
                        _msg_permission += "Danh sách bạn bè,";
                        check_permission = false;
                    }
                }

                if (!string.IsNullOrEmpty(_msg_permission) && check_permission == false)
                {
                    this.AddAlertInfo($"Bạn cần cho phép Microkol quyền đọc {_msg_permission} để đăng nhập hệ thống!");
                    await SignOut();
                    return RedirectToAction("Login");
                }

            }
            //#################################################################################################


            var loginInfo = await _facebookHelper.GetLoginProviderAsync(token);
            if (loginInfo == null)
            {
                this.AddAlertDanger($"Lỗi khi lấy thông tin từ hệ thống. Xin vui lòng thử lại. Token {token}");
                return RedirectToAction("Login");
            }

            //var accountProvider = await _accountService.GetAccountProviderByProvider(AccountProviderNames.Facebook, loginInfo.ProviderId, token);
            //var accountProviderExist = accountProvider != null;

            var auth = await _accountService.GetAuth(loginInfo);



            if (auth == null)
            {
                await SignOut();
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


            //update info facebook 
            BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbInfo(auth.Id));
            BackgroundJob.Enqueue<IFacebookJob>(m => m.UpdateFbPost(auth.Id, "system", 1)); //update fb post

            await SignIn(auth);
            //CurrentUser.AccessToken = loginInfo.AccessToken; //gan accesstoken 

            var status = await _accountService.GetAccountStatus(auth.Id);
            if(status== AccountStatus.NeedVerified)
            {
                return RedirectToAction("ChangeFacebookUrl", "Account");
            }

            return RedirectToAction("Index", "Home");
        }

      
        #endregion


        #region 
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