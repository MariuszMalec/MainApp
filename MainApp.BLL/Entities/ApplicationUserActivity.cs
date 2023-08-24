using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Http;

namespace MainApp.BLL.Entities
{
    public class ApplicationUserActivity : Entity
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public ActivityActions Action { get; set; }
        public HttpContext HttpContext { get; set; }

    }
}
