using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebInfluencer.Code;
using WebServices.Code;
using WebServices.Interfaces;

namespace WebInfluencer.Code.Middlewares
{
   
    public class AppSettingsMiddleware : IMiddleware
    {
        private readonly ISharedService _sharedService;
        public AppSettingsMiddleware(ISharedService sharedService)
        {
            _sharedService = sharedService;



        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Items[SharedConstants.APP_SETTING_KEY] == null)
            {
                context.Items[SharedConstants.APP_SETTING_KEY] = await _sharedService.GetSetting();
            }

            await next(context);
        }
    }
}
