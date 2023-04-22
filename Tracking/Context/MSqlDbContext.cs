using MainApp.BLL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tracking.Models;

namespace Tracking.Context
{
    public class MSqlDbContext : MainApplicationContext
    {
        protected readonly IConfiguration Configuration;

        public MSqlDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Provider.SqliteServer.ToString();
            options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("SqlServer"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<Event>().HasKey(e => e.Id);
            base.OnModelCreating(modelBuilder);
        }

    }
}
