using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp.BLL.Context
{
    public class PostgresDbContext : ApplicationDbContext
    {
        public PostgresDbContext(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("Postgres"));
        }
    }
}
