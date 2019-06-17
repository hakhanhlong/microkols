using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Code.Helpers;
using Website.Code.Middlewares;
using Website.Interfaces;
using Website.Jobs;
using Website.Services;

namespace Website.Code
{
    public static class AppExtensions
    {
        public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppSettingsMiddleware>();
        }



        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<ICampaignAccountRepository, CampaignAccountRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISharedService, SharedService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<INotificationService, NotificationService>();




            services.AddScoped<ICampaignJob, CampaignJob>();
            services.AddScoped<IFacebookJob, FacebookJob>();

            
            services.AddSingleton<IFileHelper, FileHelper>();
            services.AddSingleton<IFacebookClient, FacebookClient>();
            services.AddSingleton<IFacebookHelper, FacebookHelper>();
            services.AddTransient<AppSettingsMiddleware>();
        }
    }


}
