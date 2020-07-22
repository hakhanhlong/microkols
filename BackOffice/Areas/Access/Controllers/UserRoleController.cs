﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackOffice.Areas.Access.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Security.Data;
using BackOffice.Security;

namespace BackOffice.Areas.Access.Controllers
{
    [Area("Access")]
    [DisplayName("User Role Management")]
    public class UserRoleController : Controller
    {
        private readonly AppIdentityDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public UserRoleController(
            AppIdentityDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // GET: Access
        [DisplayName("User List")]
        public async Task<ActionResult> Index()
        {
            var query = await (
                    from user in _dbContext.Users
                    join ur in _dbContext.UserRoles on user.Id equals ur.UserId into userRoles
                    from userRole in userRoles.DefaultIfEmpty()
                    join rle in _dbContext.Roles on userRole.RoleId equals rle.Id into roles
                    from role in roles.DefaultIfEmpty()
                    select new { user, userRole, role }
                ).ToListAsync();

            var userList = new List<UserRoleViewModel>();
            foreach (var grp in query.GroupBy(q => q.user.Id))
            {
                var first = grp.First();
                userList.Add(new UserRoleViewModel
                {
                    UserId = first.user.Id,
                    UserName = first.user.UserName,
                    Roles = first.role != null ? grp.Select(g => g.role).Select(r => r.Name) : new List<string>()
                });
            }

            return View(userList);
        }

        // GET: Access/Edit
        [DisplayName("Edit User Roles")]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = userRoles
            };

            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["Roles"] = roles;

            return View(userViewModel);
        }

        // POST: Access/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View(viewModel);
            }

            var user = _dbContext.Users.Find(viewModel.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, viewModel.Roles);

            return RedirectToAction("Index");
        }
    }
}