using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Models;
using BackOffice.Security.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackOffice.Security.Data
{
    public class AppIdentityDbContext: IdentityDbContext<AppUser>
    {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<AppUser> _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            string _username = configuration["Data:AdminUser:UserName"];
            string _password = configuration["Data:AdminUser:Password"];
            string _email = configuration["Data:AdminUser:Email"];

            if(await _userManager.FindByNameAsync(_username) == null)
            {
                

                AppUser _appUser = new AppUser()
                {
                    UserName = _username,
                    Email = _email
                };

                IdentityResult result = await _userManager.CreateAsync(_appUser, _password);
                

            }

        }

    }
    
}
