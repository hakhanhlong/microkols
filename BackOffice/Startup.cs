using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BackOffice.Security;
using BackOffice.Security.Data;
using Infrastructure.Data;
using BackOffice.CommonHelpers;

namespace BackOffice
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

            AppConstants.RESOURCE_SERVER = Configuration["AppHelpers:RESOURCE_SERVER"];
            AppConstants.RESOURCE_PATH = Configuration["AppHelpers:RESOURCE_PATH"];

            AppHelpers.RESOURCE_SERVER = Configuration["AppHelpers:RESOURCE_SERVER"];





            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });

            services.Configure<CookieTempDataProviderOptions>(options => {
                options.Cookie.IsEssential = true;
            });


                                             

            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Database:MicroKOLsSecurity:ConnectionString"]));


            var connection = Configuration.GetConnectionString("AppContext");

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

            //services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultUI().AddDefaultTokenProviders();
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options=>options.LoginPath = "/Authen/Login");

            services.AddAppServices();

            services.AddMemoryCache();

            services.AddSession();
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
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();

        }
    }
}
