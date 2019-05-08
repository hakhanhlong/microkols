﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IAgencyService _agencyService;
        private readonly ISharedService _sharedService;
        public AuthController(IAccountService accountService, ISharedService sharedService, IAgencyService agencyService)
        {

            _accountService = accountService;
            _sharedService = sharedService;


        }


        #region Account

        #region Login
        [Route("~/dang-nhap")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("~/dang-nhap")]
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


        [Authorize]
        [Route("~/thoat")]
        public async Task<IActionResult> Logout()
        {
            await SignOut();
            return RedirectToAction("Login");
        }

        #endregion


        #region Social login

        public async Task<IActionResult> GetUserInfo(string provider, string token)
        {
            var loginInfo = provider == "Facebook" ? await Code.Helpers.SocialHelper.VerifyFacebookTokenAsync(token) :
                 await Code.Helpers.SocialHelper.VerifyGoogleTokenAsync(token);
            if (loginInfo == null)
            {
                this.AddAlertDanger($"Lỗi khi lấy thông tin từ hệ thống {provider}. Xin vui lòng thử lại. Token {token}");
                return RedirectToAction("Login");
            }
            var auth = await _accountService.GetAuth(loginInfo);
            if (auth == null)
            {

                this.AddAlertDanger("Lỗi khi lấy thông tin đăng nhập. Tài khoản đã bị khóa hoặc xóa. Xin vui lòng liên hệ quản trị hệ thống");
                return RedirectToAction("Login");
            }

            await SignIn(auth);
            return RedirectToAction("Index", "Home");
        }


        #endregion

        #endregion

        #region Agency

        #region Login
        public IActionResult AgencyLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgencyLogin(LoginViewModel model, string returnurl = "")
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

        #region Register
        public IActionResult AgencyRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AgencyRegister(CreateAgencyViewModel model, string returnurl = "")
        {
            if (ModelState.IsValid)
            {
                var id = await _agencyService.CreateAgency(model);

                if (id>0)
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
            if (!await _agencyService.VerifyUsername(username))
            {
                return Json(true);
            }

            return Json(true);
        }

        #endregion
    }
}