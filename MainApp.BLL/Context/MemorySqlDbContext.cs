using MainApp.BLL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp.BLL.Context
{
    public class MemorySqlDbContext : ApplicationDbContext
    {
        public MemorySqlDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Provider.UnitTests.ToString();
            options.UseInMemoryDatabase("MainWebDb");
        }
    }
}
