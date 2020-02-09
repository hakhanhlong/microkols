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

namespace WebMerchant.Controllers
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
        public IActionResult  Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Login(AgencyLoginViewModel model, string returnurl = "")
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAgencyViewModel model, string returnurl = "")
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

        public async Task<IActionResult> VerifyUsername(string username)
        {
            if (username.Contains("google") || username.Contains("yahoo") || username.Contains("gmail"))
            {
                return Json(false);
            }
            var r = await _agencyService.VerifyUsername(username);

            return Json(r);

        }





        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await SignOut();
            return RedirectToAction("Login");
        }
    }
}