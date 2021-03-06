﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebServices.Code;
using WebServices.Jobs;

namespace WebBgJob
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("AppContext");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

            var hangfireConnectionString = Configuration.GetConnectionString("AppHangfireContext");
            services.AddHangfire(options => options.UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true,
                SchemaName = "WebBgJob"
            }));
            services.AddAppServices();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();
            app.UseHangfireServer();


            // A Long them Local Time Zone de theo gio VN
            var campaignProcessTimer = Configuration.GetValue<string>("CampaignProcessTimer");

            RecurringJob.AddOrUpdate<ICampaignJob>(m => m.UpdateCampaignProcess(), campaignProcessTimer, TimeZoneInfo.Local); // "*/10 * * * *"   
            
            var updateFbPostTimer = Configuration.GetValue<string>("UpdateFbPostTimer");

            RecurringJob.AddOrUpdate<IFacebookJob>(m => m.UpdateFbPost(), updateFbPostTimer, TimeZoneInfo.Local);


            RecurringJob.AddOrUpdate<IFacebookJob>(m => m.UpdateFbInfo(), "*/20 * * * *", TimeZoneInfo.Local);

            RecurringJob.AddOrUpdate<ICampaignJob>(m => m.CheckLockedCampagin(), "*/5 * * * *", TimeZoneInfo.Local);

            //#################################################################################################################


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
