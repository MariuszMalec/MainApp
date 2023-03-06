using MainApp.BLL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Context
{
    public class SqliteDbContext : ApplicationDbContext
    {
        public SqliteDbContext(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var provider = Provider.SqliteServer.ToString();
            options.UseSqlite(Configuration.GetConnectionString(provider));
        }
    }
}
