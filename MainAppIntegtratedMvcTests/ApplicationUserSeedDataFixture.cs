using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRoles, int>
        {
            public override DbSet<ApplicationUser> Users { get; set; }

            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                //optionsBuilder.UseSqlite("Data Source=C:\\Temp\\Databases\\ApplicationUsers.db");//TODO to samo w appsettings.json jest

            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
