using BackOffice.Security.Data;
using DynamicAuthorization.Mvc.Core;
using DynamicAuthorization.Mvc.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.TagHelpers
{
    [HtmlTargetElement("secure-content")]
    public class SecureContentTagHelper : TagHelper
    {
        private readonly AppIdentityDbContext _dbContext;
        private readonly DynamicAuthorizationOptions _authorizationOptions;
        private readonly IRoleAccessStore _roleAccessStore;

        public SecureContentTagHelper(AppIdentityDbContext dbContext, DynamicAuthorizationOptions __DynamicAuthorizationOptions, IRoleAccessStore __IRoleAccessStore)
        {
            _dbContext = dbContext;
            _authorizationOptions = __DynamicAuthorizationOptions;
            _roleAccessStore = __IRoleAccessStore;

        }

        [HtmlAttributeName("asp-area")]
        public string Area { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var user = ViewContext.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                output.SuppressOutput();
                return;
            }

            if (user.Identity.Name.Equals(_authorizationOptions.DefaultAdminUser, StringComparison.CurrentCultureIgnoreCase))
                return;

            var actionId = $"{Area}:{Controller}:{Action}";
         
                

            var roles = await (
                from usr in _dbContext.Users
                join userRole in _dbContext.UserRoles on usr.Id equals userRole.UserId
                join role in _dbContext.Roles on userRole.RoleId equals role.Id
                where usr.UserName == user.Identity.Name
                select role.Id.ToString()
            ).ToArrayAsync();

            if (await _roleAccessStore.HasAccessToActionAsync(actionId, roles))
                return;

            output.SuppressOutput();
        }
    }
}
