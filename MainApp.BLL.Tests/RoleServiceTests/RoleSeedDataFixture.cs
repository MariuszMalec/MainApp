using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp_BLL.Tests.RoleServiceTests
{
    public class RoleSeedDataFixture : IDisposable
    {
        public ApplicationDbContext context { get; private set; }

        public RoleSeedDataFixture()
        {

            ConfigurationManager configuration = new ConfigurationManager();

            context = new MemoryDbContext(configuration);

            context.Roles.Add(new ApplicationRoles
            {
                Id  = 1,
                Name = "Test",
                NormalizedName = "TEST",
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
