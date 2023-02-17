using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using Microsoft.EntityFrameworkCore;

namespace MainAppIntegtratedMvcTests
{
    public class ApplicationUserSeedDataFixture : IDisposable
    {
        public ApplicationDbContext UserContext { get; private set; }

        public ApplicationUserSeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UsersDb")
                .Options;

            UserContext = new ApplicationDbContext(options);

            UserContext.Users.Add(new ApplicationUser
            {
                UserName = "Admin@example.com",
                Email = "Admin@example.com",
                FirstName = "Admin",
                LastName = "Admin",
                Created = DateTime.UtcNow,
                UserRole = Roles.Admin.ToString()
            });
            UserContext.SaveChanges();
        }

        public void Dispose()
        {
            UserContext.Database.EnsureDeleted();
            UserContext.Dispose();
        }
    }
}
