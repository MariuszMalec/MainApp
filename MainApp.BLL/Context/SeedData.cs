﻿using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace MainApp.BLL.Context
{
    public class SeedData
    {

        public static async void SeedClient(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRoles> roleManager)
        {
            if (context.Users.Any())
            {
                return;
            }

            var admin = new ApplicationUser()
            {
                UserName = "Admin@example.com",
                Email = "Admin@example.com",
                FirstName = "Admin",
                LastName = "Admin",
                Created = DateTime.Now
            };

            var result = await userManager.CreateAsync(admin, "Admin@13");
            if (result.Succeeded)
            {
                await roleManager.CreateAsync(new ApplicationRoles() { Name = Roles.Admin.ToString() });
                await roleManager.CreateAsync(new ApplicationRoles() { Name = Roles.User.ToString() });
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

    }
}
