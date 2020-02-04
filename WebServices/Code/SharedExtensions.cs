using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.Services;

namespace WebServices.Code
{
    public static class SharedExtensions
    {
   



        public static void AddSharedServices(this IServiceCollection services)
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
            services.AddScoped<IAccountFbPostRepository, AccountFbPostRepository>();
            

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
        }
    }


}
