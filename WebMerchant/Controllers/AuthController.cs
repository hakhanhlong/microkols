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
using Common;

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

                    try {
                        
                        //return RedirectToAction("AgencyRegister");
                        var _agency = await _agencyService.GetAgencyById(id);
                        string from = "support@microkols.com";
                        string to = model.Username;
                        string subject = "[MICROKOLS] Xác minh tài khoản doanh nghiệp";
                        string plainText = $"Chào {model.Name},";

                        string htmlText = "<p>Bạn đã đăng ký thành công một tài khoản tại MicroKOLs <b>như một doanh nghiệp!</b></p>";
                        htmlText += "<p>Vui lòng kích hoạt tài khoản và bắt đầu tạo chiến dịch của bạn bằng cách nhấn vào đường dẫn dưới đây:</p>";
                        htmlText += "<p><a clicktracking=off href=\"https://merchant.microkols.com/auth/identityverify/?ming="+ _agency.Id.ToString() + "\">kích hoạt tài khoản tại đây</a></p>";
                        htmlText += "<p>Nếu bạn có bất kỳ thắc mắc nào, hãy liên hệ với chúng tôi để nhận được sự hỗ trợ nhanh nhất.</p>";
                        htmlText += "<p>Email: info@microkols.com </p>";
                        htmlText += "<p><b>Hotline hỗ trợ: 0975119599</b> </p>";
                        htmlText += "<p>Cảm ơn bạn đã tham gia sử dụng trang Web của chúng tôi!</p>";
                        htmlText += "<p>Trân trọng,</p>";
                        htmlText += "<p><b>Phòng Dịch Vụ Khách Hàng</b></p>";
                        htmlText += "<p>Microkols Platform</p>";
                        await SendEmailHelpers.SendEmail(from, to, subject, plainText, htmlText, model.Name);
                        this.AddAlertSuccess("Đăng ký doanh nghiệp thành công. Bạn vui lòng kiểm tra email, để xác thực tài khoản doanh nghiệp của bạn.");
                    }
                    catch(Exception ex) {
                        this.AddAlertDanger(ex.Message);
                    }
                    

                }
                else {
                    this.AddAlertDanger("Doanh nghiệp đã tồn tại, bạn hãy đăng ký một doanh nghiệp khác.");
                }
                
            }
            return View(model);
        }



        public async Task<IActionResult> IdentityVerify(int ming)
        {
            var agency = await _agencyService.GetAgencyById(ming); // agency not yet active
            if(agency != null)
            {
                var retValue = await _agencyService.VerifyEmail(agency.Id); //verify email and active agency
                if(retValue > 0) {
                    this.AddAlertSuccess("Doanh nghiệp của bạn đã được xác thực, hãy đăng nhập.");                    
                }
                else {
                    //không vào trường hợp này
                }
            }
            else
            {
                this.AddAlertDanger("Không tồn tại doanh nghiệp!");
            }

            return View();
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