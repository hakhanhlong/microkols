
using Common.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Website.Code.Extensions
{
    public static class UserExtensions
    {
       
        public static string GetAnonymousId(this HttpContext httpContext)
        {
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            return SecurityHelper.MD5Hash(remoteIpAddress + userAgent);
        }


        public static string GetUsername(this HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                return httpContext.User.Identity.Name;
            }

            return httpContext.GetAnonymousId();
        }
        
    }
}
