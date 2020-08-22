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
using Common.Helpers;

namespace WebMerchant.Controllers
{
    public class AuthController : BaseAuthController
    {
        private readonly IAccountService _accountService;
        private readonly IAgencyService _agencyService;
        private readonly ISharedService _sharedService;
        private readonly IFacebookHelper _facebookHelper;
        private readonly IFacebookJob _IFacebookJob;
        public AuthController(IAccountService accountService, ISharedService sharedService, IAgencyService agencyService, IFacebookHelper facebookHelper, IFacebookJob __IFacebookJob)
        {

            _accountService = accountService;
            _sharedService = sharedService;
            _agencyService = agencyService;
            _facebookHelper = facebookHelper;
            _IFacebookJob = __IFacebookJob;

        }

        #region Login
        public IActionResult  Login()
        {
            //await _IFacebookJob.UpdateFbPost(112, "system", 2);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Login(AgencyLoginViewModel model, string returnurl = "")
        {
            if (ModelState.IsValid)
            {
                var auth = await _agencyService.GetAuth2(model);

                if (auth != null)
                {
                    if (auth.AgencyActived)
                    {                        
                        await SignIn(auth);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this.AddAlertDanger("Tài khoản của bạn chưa được kích hoạt!");
                    }                    
                }
                else { this.AddAlertDanger("Tên đăng nhập hoặc mật khẩu không đúng"); }
                
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
                        this.AddAlertSuccess("Bạn đã đăng ký tài khoản doanh nghiệp, vui lòng kiểm tra email để xác thực thông tin.");
                    }
                    catch(Exception ex) {
                        this.AddAlertDanger(ex.Message);
                    }
                    

                }
                else {
                    this.AddAlertDanger("Tên hoặc email doanh nghiệp đã tồn tại, xin vui lòng kiểm tra lại.");
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


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var exist = await _agencyService.VerifyUsername(email);
            if (exist) {

                var agency = await _agencyService.GetByEmail(email);

                string password = await _agencyService.ChangePassword(email);

                string from = "support@microkols.com";
                string to = agency.Username;
                string subject = "[MICROKOLS] Thông tin mật khẩu của bạn";

                string plainText = $"Chào {agency.Name},";

                string htmlText = $"<p>Mật khẩu mới của bạn là: {password}</p>";
                htmlText += $"<p>Bạn hãy thay đổi mật khẩu sau khi đăng nhập thành công</p>";

                htmlText += "<p>Nếu bạn có bất kỳ thắc mắc nào, hãy liên hệ với chúng tôi để nhận được sự hỗ trợ nhanh nhất.</p>";

                htmlText += "<p>Email: info@microkols.com </p>";
                htmlText += "<p><b>Hotline hỗ trợ: 0975119599</b> </p>";
                htmlText += "<p>Cảm ơn bạn đã tham gia sử dụng trang Web của chúng tôi!</p>";
                htmlText += "<p>Trân trọng,</p>";
                htmlText += "<p><b>Phòng Dịch Vụ Khách Hàng</b></p>";
                htmlText += "<p>Microkols Platform</p>";

                await SendEmailHelpers.SendEmail(from, to, subject, plainText, htmlText, agency.Name);

                this.AddAlertDanger("Mật khẩu mới đã được gửi về email của bạn!");
            }
            else {
                this.AddAlertDanger("Không tồn tại email!");
            }

            return RedirectToAction("ForgetPassword", "Auth");
        }

    }
}