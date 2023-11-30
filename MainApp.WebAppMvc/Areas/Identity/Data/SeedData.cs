using MainApp.WebAppMvc.Data;

namespace MainApp.WebAppMvc.Areas.Identity.Data
{
    public class SeedData
    {
        public static async void SeedAdmin(MainAppWebAppMvcContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var admin = new MainAppWebAppMvcUser()
            {
                UserName = "Admin@example.com",
                Email = "Admin@example.com",
            };
            context.AddRange(admin);
            await context.SaveChangesAsync();
        }
    }
}
