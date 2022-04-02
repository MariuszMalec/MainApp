using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MainApp.BLL.Context
{
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
