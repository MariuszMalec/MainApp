using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Tracking.Context
{
    public class MySqlDbContext : MainApplicationContext
    {
        protected readonly IConfiguration Configuration;

        public MySqlDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Configuration["Provider"];

            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            switch (provider)
            {
                case "MySqlLinux":
                    options.UseMySql(Configuration.GetConnectionString("MySqlLinux"), serverVersion);
                    break;
                case "MySqlWin":
                    options.UseMySql(Configuration.GetConnectionString("MySqlWin"), serverVersion);
                    break;
            }
        }

    }
}
