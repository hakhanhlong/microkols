using BackOffice.Business;
using BackOffice.Business.Interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Code.Helpers;

namespace BackOffice
{
    public static class AppContext
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<ICampaignTypeChargeRepository, CampaignTypeChargeRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionHistoryRepository, TransactionHistoryRepository>();

            services.AddScoped<IPayoutExportRepository, PayoutExportRepository>();

            services.AddScoped<IAccountCampaignChargeRepository, AccountCampaignChargeRepository>();
            services.AddScoped<ICampaignAccountRepository, CampaignAccountRepository>();

            services.AddScoped<ISettingRepository, SettingRepository>();


            //business
            services.AddScoped<IAgencyBusiness, AgencyBusiness>();
            services.AddScoped<IWalletBusiness, WalletBusiness>();
            services.AddScoped<IAccountBusiness, AccountBusiness>();
            services.AddScoped<ITransactionBusiness, TransactionBusiness>();
            services.AddScoped<ITransactionHistoryBusiness, TransactionHistoryBusiness>();
            services.AddScoped<IAccountCampaignChargeBusiness, AccountCampaignChargeBusiness>();
            services.AddScoped<ICampaignBusiness, CampaignBusiness>();

            services.AddScoped<ISharedBusiness, SharedBusiness>();
            services.AddScoped<INotificationBusiness, NotificationBusiness>();
            


            //website
            services.AddSingleton<IFacebookClient, FacebookClient>();
            services.AddSingleton<IFacebookHelper, FacebookHelper>();

            services.AddScoped<Website.Interfaces.IAccountService, Website.Services.AccountService>();
            services.AddScoped<IAccountFbPostRepository, AccountFbPostRepository>();








        }
    }
}
