using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                if(await _roleManager.FindByNameAsync(Role.Administratror.GetRoleName()) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(Role.Administratror.GetRoleName()));
                }

                AppUser _appUser = new AppUser()
                {
                    UserName = _username,
                    Email = _email
                };

                IdentityResult result = await _userManager.CreateAsync(_appUser, _password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_appUser, Role.Administratror.GetRoleName());
                }

            }

        }

    }
    
}
