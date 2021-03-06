﻿
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.Services;

namespace WebLandingPage
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
            services.AddScoped<ICampaignAccountStatisticRepository, CampaignAccountStatisticRepository>();

            services.AddScoped<ISettingRepository, SettingRepository>();

            services.AddScoped<IVideoGalleryRepository, VideoGalleryRepository>();

            services.AddScoped<IQnARepository, QnARepository>();
            services.AddScoped<IQnAImageRepository, QnAImageRepository>();
            services.AddScoped<IQnAVideoRepository, QnAVideoRepository>();

            services.AddScoped<INotificationService, NotificationService>();

            


     



            // webservice
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IAgencyService, AgencyService>();

            services.AddScoped<IQnAService, QnAService>();

            services.AddScoped<IQnAImageService, QnAImageService>();
            services.AddScoped<IQnAVideoService, QnAVideoService>();


            services.AddScoped<IVideoGalleryService, VideoGalleryService>();

            services.AddScoped<ISharedService, SharedService>();
            services.AddScoped<IAccountService, AccountService>();

            

            services.AddScoped<ICampaignAccountCaptionService, CampaignAccountCaptionService>();
            services.AddScoped<ICampaignAccountContentService, CampaignAccountContentService>();
            services.AddScoped<ICampaignAccountStatisticService, CampaignAccountStatisticService>();

            services.AddScoped<WebServices.Code.Helpers.IFacebookClient, WebServices.Code.Helpers.FacebookClient>();
            services.AddScoped<WebServices.Code.Helpers.IFacebookHelper, WebServices.Code.Helpers.FacebookHelper>();
            














        //website




            services.AddScoped<IAccountFbPostRepository, AccountFbPostRepository>();








        }
    }
}
