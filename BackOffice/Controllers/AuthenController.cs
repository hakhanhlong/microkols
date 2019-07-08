using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackOffice.Security;
using BackOffice.Security.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace BackOffice.Controllers
{
    
    public class AuthenController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        
        public AuthenController(UserManager<AppUser> _userMgr, SignInManager<AppUser> _signinMgr)
        {
            _userManager = _userMgr;
            _signInManager = _signinMgr;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnURL)
        {
            ViewBag.returnUrl = returnURL;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(details.Email);
                if(user != null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, details.Password, true, false);
                    if (result.Succeeded)
                    {
                        return Redirect("/");
                    }
                    
                }
                //ModelState.AddModelError(nameof(LoginModel.Email), "Invalid user or Password");
                TempData["MessageError"] = "Invalid user or Password";
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {            
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel details)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.ChangePasswordAsync(user, details.OldPassword, details.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        TempData["MessageError"] += string.Format("Code {0}: {1}", error.Code, error.Description);
                    }                    
                }
                else
                {
                    TempData["MessageSuccess"] = "Change password success!";
                }
            }
        
            return View(details);
        }
    }
}