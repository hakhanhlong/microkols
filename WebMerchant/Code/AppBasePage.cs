using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;
using WebMerchant.Code.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using WebServices.Code;

namespace WebMerchant.Code
{

    public abstract class AppBasePage<TModel> : RazorPage<TModel>
    {

        public string AbsoluteUrl => $"{Context.Request.Scheme}://{Context.Request.Host}{Context.Request.Path}{Context.Request.QueryString}";

        protected AuthViewModel CurrentUser => User.Identity.IsAuthenticated ? AuthViewModel.GetModel(User) : null;

        public Core.Models.SettingModel AppSettings => (Core.Models.SettingModel)Context.Items[SharedConstants.APP_SETTING_KEY];

    }
}
