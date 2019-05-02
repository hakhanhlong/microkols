using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.ViewModels;
using Website.Code.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Website.Code
{

    public abstract class AppBasePage<TModel> : RazorPage<TModel>
    {

        public string AbsoluteUrl => $"{Context.Request.Scheme}://{Context.Request.Host}{Context.Request.Path}{Context.Request.QueryString}";

        protected AuthViewModel CurrentUser => User.Identity.IsAuthenticated ? AuthViewModel.GetModel(User) : null;

        
        
    }
}
