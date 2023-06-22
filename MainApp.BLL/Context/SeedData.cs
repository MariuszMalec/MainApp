using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.BLL.Context
{
    public class SeedData
    {

        public static async Task SeedUser(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRoles> roleManager)
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
                Created = DateTime.UtcNow,
                UserRole = Roles.Admin.ToString()
            };

            var result = await userManager.CreateAsync(admin, "Admin@13");
            if (result.Succeeded)
            {
                await roleManager.CreateAsync(new ApplicationRoles() { Name = Roles.Admin.ToString() });
                await roleManager.CreateAsync(new ApplicationRoles() { Name = Roles.User.ToString() });
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());//TODO to nie dziala!!!
                // context.AddRange(admin);
                // await context.SaveChangesAsync();                
            }
        }
        public static async void SeedAdmin(ApplicationDbContext context)
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
                Created = DateTime.UtcNow
            };
            context.AddRange(admin);
            await context.SaveChangesAsync();
        }
        public static async Task SeedRole(ApplicationDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }
            var role = new ApplicationRoles()
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            context.AddRange(role);
            await context.SaveChangesAsync();
        }
    }
}
