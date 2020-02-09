using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebServices.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace WebMerchant.Controllers
{
 
    public class BaseAuthController : Controller
    {
        protected AuthViewModel CurrentUser => User.Identity.IsAuthenticated ? AuthViewModel.GetModel(User) : null;

        protected string UrlReferrer => Request.Headers["Referer"].ToString();

        protected string IpAddress => Request.HttpContext.Connection.RemoteIpAddress.ToString();

        #region SignIn & SignOut
        protected async Task SignIn(AuthViewModel model)
        {
            var claims = model.GetClaims();
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                IsPersistent = true,
            });
        }

        protected async Task SignOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        #endregion
    }


    [Authorize]
    public class BaseController : BaseAuthController
    { 
    
    }
 }