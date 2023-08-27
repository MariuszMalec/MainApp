using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp_BLL.Tests.RoleServiceTests
{
    public class SeedDataFixture : IDisposable
    {
        public ApplicationDbContext context { get; private set; }

        public SeedDataFixture()
        {

            ConfigurationManager configuration = new ConfigurationManager();

            context = new MemoryDbContext(configuration);

            context.Roles.Add(new ApplicationRoles
            {
                Id  = 1,
                Name = "Test",
                NormalizedName = "TEST",
            });

            context.Users.Add(new ApplicationUser
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Email = "Test@example.com",
                UserRole="Test"
            });

            context.SaveChanges();
        }

        public class MemoryDbContext : ApplicationDbContext
        {
            public MemoryDbContext(IConfiguration configuration)
                : base(configuration)
            {
            }

            public override DbSet<ApplicationRoles> Roles { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                options.UseInMemoryDatabase("RolesDb");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<ApplicationRoles>().ToTable("Roles");
                base.OnModelCreating(modelBuilder);
            }
        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
