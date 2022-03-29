using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using System.Linq;

namespace MainApp.BLL.DataStorage
{
    public class LoadAdmin
    {
        public static void SeedDatabase(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var adminRole = new Role()
                {
                    Name = "Admin"
                };

                var userRole = new Role()
                {
                    Name = "User"
                };

                context.Roles.Add(adminRole);
                context.Roles.Add(userRole);

                var admin = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "",
                    Email = "admin@admin.eu",
                    Role = adminRole
                };

                var encodePassword = Base64EncodeDecode.Base64Encode("bTeamRoxs");
                admin.PasswordHash = encodePassword;
                context.Add(admin);
                context.SaveChanges();
            }
        }
    }
}
