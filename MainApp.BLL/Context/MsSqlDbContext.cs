using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp.BLL.Context
{
    public class MsSqlDbContext : ApplicationDbContext
    {
        public MsSqlDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
        }
    }
}
