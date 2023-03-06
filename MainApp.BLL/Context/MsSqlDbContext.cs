using MainApp.BLL.Enums;
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
            var provider = Provider.SqlServer.ToString();
            options.UseSqlServer(Configuration.GetConnectionString(provider));
        }
    }
}
