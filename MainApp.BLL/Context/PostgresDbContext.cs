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
            //var provider = Configuration["DatabaseProvider"];
            var provider = Configuration["Provider"];
            switch (provider)
            {
                case "PostgresWin":
                    options.UseNpgsql(Configuration.GetConnectionString("PostgresWin"));//TODO to jak dodam to dziala! jak wrzucic to do progrmam.cs
                    break;

                case "PostgresLinux":
                    options.UseNpgsql(Configuration.GetConnectionString("PostgresLinux"));//TODO to jak dodam to dziala! jak wrzucic to do progrmam.cs
                    break;
            }
        }
    }
}
