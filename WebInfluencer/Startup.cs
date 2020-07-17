using System;
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
using WebInfluencer.Code;
using WebServices.Code;

namespace WebInfluencer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SharedOptions>(Configuration.GetSection("SharedOptions"));



            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/AccessDenied/";
                options.LoginPath = "/Auth/Login/";
                options.LogoutPath = "/Auth/Logout/";
                options.Cookie.Name = "MicroKolInfluencerCookie";
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;

            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["SharedOptions:FacebookAppId"];
                facebookOptions.AppSecret = Configuration["SharedOptions:FacebookAppSecret"];
                facebookOptions.SaveTokens = true;
                facebookOptions.Scope.Add("public_profile");
                facebookOptions.Scope.Add("email");
                facebookOptions.Scope.Add("user_link");
                facebookOptions.Scope.Add("user_friends");
                facebookOptions.Scope.Add("user_posts");

                

                facebookOptions.Events = new OAuthEvents()
                {
                    OnRemoteFailure = ctx =>
                    {
                        var authProperties = facebookOptions.StateDataFormat.Unprotect(ctx.Request.Query["state"]);
                        // doc something

                        ctx.HandleResponse();
                        ctx.Response.Redirect("/auth/signinfacebook");
                        ctx.HandleResponse();

                        return Task.FromResult(0);
                    }
                };

                
            });

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
                SchemaName = "Influencer"
            }));
            services.AddAppServices();

            services.AddSession();
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAppMiddlewares();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
