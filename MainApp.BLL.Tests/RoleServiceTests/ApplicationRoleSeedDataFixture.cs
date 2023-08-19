using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MainApp_BLL.Tests.RoleServiceTests
{
    public class ApplicationRoleSeedDataFixture : IDisposable
    {
        public ApplicationDbContext RoleContext { get; private set; }

        public ApplicationRoleSeedDataFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("RolesDb")
                .Options;

            RoleContext = new ApplicationDbContext(options);

            RoleContext.Roles.Add(new ApplicationRoles
            {
                Id = 1,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            });
            RoleContext.SaveChanges();
        }

        public void Dispose()
        {
            RoleContext.Database.EnsureDeleted();
            RoleContext.Dispose();
        }

        public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRoles, int>
        {
            public override DbSet<ApplicationRoles> Roles { get; set; }

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
                modelBuilder.Entity<ApplicationRoles>().ToTable("ApplicationRoles");
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
