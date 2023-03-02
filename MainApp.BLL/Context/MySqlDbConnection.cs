using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MainApp.BLL.Context
{
    public class MySqlDbContext : ApplicationDbContext
    {
        public MySqlDbContext(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Configuration["DatabaseProvider"];
            switch (provider)
            {
                case "MySqlLinux":
                    var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
                    options.UseMySql(Configuration.GetConnectionString("MySqlLinux"), serverVersion);
                    break;
            }
        }
    }
}