using MainApp.BLL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tracking.Context
{
    public class MemorySqlDbContext : MainApplicationContext
    {
        protected readonly IConfiguration Configuration;

        public MemorySqlDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Provider.UnitTests.ToString();
            options.UseInMemoryDatabase("TrackingDb");
        }
    }

}
