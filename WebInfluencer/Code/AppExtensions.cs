using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Code.Helpers;
using WebInfluencer.Code.Middlewares;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.Services;
using WebServices.Code;

namespace WebInfluencer.Code
{
    public static class AppExtensions
    {
        public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppSettingsMiddleware>();
        }



        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddSharedServices();
            services.AddTransient<AppSettingsMiddleware>();
        }
    }


}
