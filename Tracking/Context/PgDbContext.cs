using MainApp.BLL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tracking.Context
{
    public class PgDbContext : MainApplicationContext
    {
        protected readonly IConfiguration Configuration;

        public PgDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Provider.PostgresWin.ToString();
            //options.UseNpgsql(Configuration.GetConnectionString(provider));
        }

    }
}
