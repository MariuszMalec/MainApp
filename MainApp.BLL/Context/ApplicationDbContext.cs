using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp.BLL.Context
{
    public abstract class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRoles, int>
    {
        public override DbSet<ApplicationUser> Users { get; set; }

        protected readonly IConfiguration Configuration;

        protected ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options)
        //{
        //}
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //    //optionsBuilder.UseSqlite("Data Source=C:\\Temp\\Databases\\ApplicationUsers.db");//TODO to samo w appsettings.json jest
        //    optionsBuilder.UseNpgsql(Configuration.GetConnectionString("PostgresLinux"));//TODO to jak dodam to dziala! jak wrzucic to do progrmam.cs
        // }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
